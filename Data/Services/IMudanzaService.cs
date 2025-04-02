using MudanzaApp.Data.Models;

namespace MudanzaApp.Data.Services
{
    public interface IMudanzaService
    {
        // Operaciones de Mudanza
        Task<IEnumerable<Mudanza>> GetUserMudanzasAsync(string userId);
        Task<Mudanza> GetMudanzaByIdAsync(int id);
        Task<Mudanza> GetMudanzaBySharingCodeAsync(string sharingCode);
        Task<Mudanza> CreateMudanzaAsync(Mudanza mudanza);
        Task<bool> UpdateMudanzaAsync(Mudanza mudanza);
        Task<bool> DeleteMudanzaAsync(int id);

        // Cambios de estado
        Task<bool> ChangeStatusAsync(int mudanzaId, MudanzaStatus newStatus, string userId, string comments = null);
        Task<bool> SubmitForReviewAsync(int mudanzaId, string userId);
        Task<bool> ReopenAsDraftAsync(int mudanzaId, string userId);

        // Validaciones
        Task<bool> CanEditMudanzaAsync(int mudanzaId, string userId);
        Task<bool> IsOwnerAsync(int mudanzaId, string userId);
        Task<bool> IsCollaboratorAsync(int mudanzaId, string userId);

        // Items de mudanza
        Task<MudanzaItem> AddItemAsync(MudanzaItem item, string userId);
        Task<bool> UpdateItemAsync(MudanzaItem item);
        Task<bool> DeleteItemAsync(int itemId);

        // Colaboradores
        Task<MudanzaCollaborator> AddCollaboratorAsync(int mudanzaId, string name, string email = null);
        Task<bool> RemoveCollaboratorAsync(int collaboratorId);
        Task<bool> UpdateCollaboratorAsync(MudanzaCollaborator collaborator);

        // Comentarios
        Task<MudanzaComment> AddCommentAsync(MudanzaComment comment);

        // Para administradores
        Task<IEnumerable<Mudanza>> GetAllMudanzasAsync(MudanzaStatus? status = null);
        Task<bool> UpdateEstimatedCostAsync(int mudanzaId, decimal cost);
        Task<bool> UpdateFinalCostAsync(int mudanzaId, decimal cost);
    }

}
