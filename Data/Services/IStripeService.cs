// MudanzaApp/Data/Services/IStripeService.cs
namespace MudanzaApp.Data.Services
{
    public interface IStripeService
    {
        Task<string> CreateCheckoutSessionAsync(string userId, int credits, decimal amount);
        Task<PhotoCreditPackage[]> GetPhotoCreditPackagesAsync();
    }

    public class PhotoCreditPackage
    {
        public int Credits { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsMostPopular { get; set; }
    }
}

