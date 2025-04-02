using Microsoft.EntityFrameworkCore;
using MudanzaApp.Data.Models;

namespace MudanzaApp.Data.Services.Impl
{
    public class MudanzaService : IMudanzaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MudanzaService> _logger;

        public MudanzaService(ApplicationDbContext context, ILogger<MudanzaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Mudanza>> GetUserMudanzasAsync(string userId)
        {
            return await _context.Mudanzas
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Mudanza> GetMudanzaByIdAsync(int id)
        {
            return await _context.Mudanzas
                .Include(m => m.Items)
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                .Include(m => m.Collaborators)
                .Include(m => m.StatusHistory)
                    .ThenInclude(h => h.ChangedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Mudanza> GetMudanzaBySharingCodeAsync(string sharingCode)
        {
            return await _context.Mudanzas
                .Include(m => m.Items)
                .Include(m => m.Collaborators)
                .FirstOrDefaultAsync(m => m.SharingCode == sharingCode);
        }

        public async Task<Mudanza> CreateMudanzaAsync(Mudanza mudanza)
        {
            _context.Mudanzas.Add(mudanza);

            // Crear un registro en el historial de estados
            var statusHistory = new MudanzaStatusHistory
            {
                Mudanza = mudanza,
                Status = mudanza.Status,
                ChangedByUserId = mudanza.UserId,
                ChangedAt = DateTime.UtcNow,
                Comments = "Mudanza creada"
            };

            _context.MudanzaStatusHistory.Add(statusHistory);

            await _context.SaveChangesAsync();
            return mudanza;
        }

        public async Task<bool> UpdateMudanzaAsync(Mudanza mudanza)
        {
            mudanza.UpdatedAt = DateTime.UtcNow;
            _context.Mudanzas.Update(mudanza);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteMudanzaAsync(int id)
        {
            var mudanza = await _context.Mudanzas.FindAsync(id);
            if (mudanza == null)
                return false;

            _context.Mudanzas.Remove(mudanza);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ChangeStatusAsync(int mudanzaId, MudanzaStatus newStatus, string userId, string comments = null)
        {
            var mudanza = await _context.Mudanzas.FindAsync(mudanzaId);
            if (mudanza == null)
                return false;

            // Validar transiciones de estado
            if (!IsValidStatusTransition(mudanza.Status, newStatus))
            {
                _logger.LogWarning($"Invalid status transition from {mudanza.Status} to {newStatus} for mudanza {mudanzaId}");
                return false;
            }

            var oldStatus = mudanza.Status;
            mudanza.Status = newStatus;
            mudanza.UpdatedAt = DateTime.UtcNow;

            // Si se está cerrando el borrador
            if (newStatus == MudanzaStatus.DraftClosed && oldStatus == MudanzaStatus.DraftOpen)
            {
                mudanza.ClosedAt = DateTime.UtcNow;
            }
            // Si se está enviando a revisión
            else if (newStatus == MudanzaStatus.InReview)
            {
                mudanza.SubmittedAt = DateTime.UtcNow;
            }

            // Registrar el cambio en el historial
            var statusHistory = new MudanzaStatusHistory
            {
                MudanzaId = mudanzaId,
                Status = newStatus,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow,
                Comments = comments
            };

            _context.MudanzaStatusHistory.Add(statusHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubmitForReviewAsync(int mudanzaId, string userId)
        {
            return await ChangeStatusAsync(mudanzaId, MudanzaStatus.InReview, userId, "Mudanza enviada para revisión");
        }

        public async Task<bool> ReopenAsDraftAsync(int mudanzaId, string userId)
        {
            return await ChangeStatusAsync(mudanzaId, MudanzaStatus.DraftOpen, userId, "Mudanza reabierta como borrador");
        }

        public async Task<bool> CanEditMudanzaAsync(int mudanzaId, string userId)
        {
            var mudanza = await _context.Mudanzas
                .Include(m => m.Collaborators)
                .FirstOrDefaultAsync(m => m.Id == mudanzaId);

            if (mudanza == null)
                return false;

            // Si es el propietario
            if (mudanza.UserId == userId)
            {
                // Solo el propietario puede editar si está en borrador
                return mudanza.Status == MudanzaStatus.DraftOpen || mudanza.Status == MudanzaStatus.DraftClosed;
            }

            // Si es colaborador, solo puede editar si está en borrador abierto
            var isCollaborator = mudanza.Collaborators.Any(c => c.UserId == userId);
            return isCollaborator && mudanza.Status == MudanzaStatus.DraftOpen;
        }

        public async Task<bool> IsOwnerAsync(int mudanzaId, string userId)
        {
            var mudanza = await _context.Mudanzas.FindAsync(mudanzaId);
            return mudanza != null && mudanza.UserId == userId;
        }

        public async Task<bool> IsCollaboratorAsync(int mudanzaId, string userId)
        {
            return await _context.MudanzaCollaborators
                .AnyAsync(c => c.MudanzaId == mudanzaId && c.UserId == userId);
        }

        public async Task<MudanzaItem> AddItemAsync(MudanzaItem item, string userId)
        {
            // Verificar si la mudanza puede ser editada
            var canEdit = await CanEditMudanzaAsync(item.MudanzaId, userId);
            if (!canEdit)
                return null;

            _context.MudanzaItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateItemAsync(MudanzaItem item)
        {
            _context.MudanzaItems.Update(item);
            item.UpdatedAt = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteItemAsync(int itemId)
        {
            var item = await _context.MudanzaItems.FindAsync(itemId);
            if (item == null)
                return false;

            _context.MudanzaItems.Remove(item);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<MudanzaCollaborator> AddCollaboratorAsync(int mudanzaId, string name, string email = null)
        {
            var collaborator = new MudanzaCollaborator
            {
                MudanzaId = mudanzaId,
                CollaboratorName = name,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            // Si hay un email, intentar encontrar el usuario
            if (!string.IsNullOrEmpty(email))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    collaborator.UserId = user.Id;
                }
            }

            _context.MudanzaCollaborators.Add(collaborator);
            await _context.SaveChangesAsync();
            return collaborator;
        }

        public async Task<bool> RemoveCollaboratorAsync(int collaboratorId)
        {
            var collaborator = await _context.MudanzaCollaborators.FindAsync(collaboratorId);
            if (collaborator == null)
                return false;

            _context.MudanzaCollaborators.Remove(collaborator);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateCollaboratorAsync(MudanzaCollaborator collaborator)
        {
            _context.MudanzaCollaborators.Update(collaborator);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<MudanzaComment> AddCommentAsync(MudanzaComment comment)
        {
            _context.MudanzaComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Mudanza>> GetAllMudanzasAsync(MudanzaStatus? status = null)
        {
            // Construir la consulta base
            IQueryable<Mudanza> baseQuery = _context.Mudanzas;

            // Aplicar la inclusión
            baseQuery = baseQuery.Include(m => m.User);

            // Aplicar filtro condicional
            if (status.HasValue)
            {
                baseQuery = baseQuery.Where(m => m.Status == status.Value);
            }

            // Aplicar ordenamiento y ejecutar
            return await baseQuery.OrderByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task<bool> UpdateEstimatedCostAsync(int mudanzaId, decimal cost)
        {
            var mudanza = await _context.Mudanzas.FindAsync(mudanzaId);
            if (mudanza == null)
                return false;

            mudanza.EstimatedCost = cost;
            mudanza.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateFinalCostAsync(int mudanzaId, decimal cost)
        {
            var mudanza = await _context.Mudanzas.FindAsync(mudanzaId);
            if (mudanza == null)
                return false;

            mudanza.FinalCost = cost;
            mudanza.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // Método auxiliar para validar transiciones de estado
        private bool IsValidStatusTransition(MudanzaStatus currentStatus, MudanzaStatus newStatus)
        {
            // Permitir transiciones de estado válidas
            switch (currentStatus)
            {
                case MudanzaStatus.DraftOpen:
                    return newStatus == MudanzaStatus.DraftClosed;

                case MudanzaStatus.DraftClosed:
                    return newStatus == MudanzaStatus.DraftOpen || newStatus == MudanzaStatus.InReview;

                case MudanzaStatus.InReview:
                    return newStatus == MudanzaStatus.DraftOpen ||
                           newStatus == MudanzaStatus.WaitingForDocuments ||
                           newStatus == MudanzaStatus.ReceivedInWarehouseUS;

                case MudanzaStatus.WaitingForDocuments:
                    return newStatus == MudanzaStatus.InReview ||
                           newStatus == MudanzaStatus.ReceivedInWarehouseUS;

                case MudanzaStatus.ReceivedInWarehouseUS:
                    return newStatus == MudanzaStatus.WaitingForCrossing ||
                           newStatus == MudanzaStatus.ReceivedInWarehouseMX;

                case MudanzaStatus.ReceivedInWarehouseMX:
                    return newStatus == MudanzaStatus.WaitingForShipment ||
                           newStatus == MudanzaStatus.InTransit;

                case MudanzaStatus.WaitingForCrossing:
                    return newStatus == MudanzaStatus.ReceivedInWarehouseUS ||
                           newStatus == MudanzaStatus.ReceivedInWarehouseMX;

                case MudanzaStatus.WaitingForShipment:
                    return newStatus == MudanzaStatus.InTransit;

                case MudanzaStatus.InTransit:
                    return newStatus == MudanzaStatus.Delivered;

                case MudanzaStatus.Delivered:
                    return newStatus == MudanzaStatus.Completed;

                default:
                    return false;
            }
        }
    }
}

