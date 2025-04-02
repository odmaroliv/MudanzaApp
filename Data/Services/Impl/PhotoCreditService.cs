using Microsoft.EntityFrameworkCore;
using MudanzaApp.Data.Models;

namespace MudanzaApp.Data.Services.Impl
{
    public class PhotoCreditService : IPhotoCreditService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PhotoCreditService> _logger;

        public PhotoCreditService(ApplicationDbContext context, ILogger<PhotoCreditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetAvailableCreditsAsync(string userId)
        {
            var photoCredit = await GetOrCreatePhotoCreditAsync(userId);
            return photoCredit.AvailableCredits;
        }

        public async Task<bool> UsePhotoCreditsAsync(string userId, int credits = 1)
        {
            var photoCredit = await GetOrCreatePhotoCreditAsync(userId);

            if (photoCredit.AvailableCredits < credits)
            {
                _logger.LogWarning($"User {userId} tried to use {credits} credits but only has {photoCredit.AvailableCredits}");
                return false;
            }

            photoCredit.AvailableCredits -= credits;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User {userId} used {credits} photo credits. Remaining: {photoCredit.AvailableCredits}");
            return true;
        }

        public async Task<PhotoCreditPurchase> CreatePurchaseAsync(string userId, int creditsAmount, decimal amount)
        {
            var purchase = new PhotoCreditPurchase
            {
                UserId = userId,
                CreditsAmount = creditsAmount,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            };

            _context.PhotoCreditPurchases.Add(purchase);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created purchase of {creditsAmount} credits for user {userId}");
            return purchase;
        }

        public async Task<bool> CompletePurchaseAsync(string stripeSessionId)
        {
            var purchase = await _context.PhotoCreditPurchases
                .FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId && !p.IsCompleted);

            if (purchase == null)
            {
                _logger.LogWarning($"Purchase with session ID {stripeSessionId} not found or already completed");
                return false;
            }

            purchase.IsCompleted = true;
            purchase.CompletedAt = DateTime.UtcNow;

            // Añadir créditos al usuario
            var photoCredit = await GetOrCreatePhotoCreditAsync(purchase.UserId);
            photoCredit.AvailableCredits += purchase.CreditsAmount;
            photoCredit.LastPurchase = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Completed purchase of {purchase.CreditsAmount} credits for user {purchase.UserId}");
            return true;
        }

        public async Task<PhotoCreditPurchase> GetPurchaseBySessionIdAsync(string sessionId)
        {
            return await _context.PhotoCreditPurchases
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId);
        }

        public async Task<bool> UpdatePurchaseSessionIdAsync(int purchaseId, string sessionId)
        {
            var purchase = await _context.PhotoCreditPurchases
                .FirstOrDefaultAsync(p => p.Id == purchaseId);

            if (purchase == null)
            {
                _logger.LogWarning($"Purchase with ID {purchaseId} not found");
                return false;
            }

            purchase.StripeSessionId = sessionId;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Updated purchase {purchaseId} with Stripe session ID {sessionId}");
            return true;
        }

        // Método auxiliar para obtener o crear un registro de créditos de fotos
        private async Task<PhotoCredit> GetOrCreatePhotoCreditAsync(string userId)
        {
            var photoCredit = await _context.PhotoCredits
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (photoCredit == null)
            {
                photoCredit = new PhotoCredit
                {
                    UserId = userId,
                    AvailableCredits = 5, // 5 créditos gratuitos iniciales
                    LastPurchase = DateTime.UtcNow
                };

                _context.PhotoCredits.Add(photoCredit);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Created new photo credit record for user {userId} with 5 initial credits");
            }

            return photoCredit;
        }
    }
}

