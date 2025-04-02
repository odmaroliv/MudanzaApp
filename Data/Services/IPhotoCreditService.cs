using MudanzaApp.Data.Models;

namespace MudanzaApp.Data.Services
{
    public interface IPhotoCreditService
    {
        Task<int> GetAvailableCreditsAsync(string userId);
        Task<bool> UsePhotoCreditsAsync(string userId, int credits = 1);
        Task<PhotoCreditPurchase> CreatePurchaseAsync(string userId, int creditsAmount, decimal amount);
        Task<bool> CompletePurchaseAsync(string stripeSessionId);
        Task<PhotoCreditPurchase> GetPurchaseBySessionIdAsync(string sessionId);
        Task<bool> UpdatePurchaseSessionIdAsync(int purchaseId, string sessionId);
    }
}
