
using MudanzaApp.Data.Services;
using Stripe;
using Stripe.Checkout;

public class StripeService : IStripeService
{
    private readonly IConfiguration _configuration;
    private readonly IPhotoCreditService _photoCreditService;
    private readonly ILogger<StripeService> _logger;

    public StripeService(
        IConfiguration configuration,
        IPhotoCreditService photoCreditService,
        ILogger<StripeService> logger)
    {
        _configuration = configuration;
        _photoCreditService = photoCreditService;
        _logger = logger;

        // Configurar la clave secreta de Stripe
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public async Task<string> CreateCheckoutSessionAsync(string userId, int credits, decimal amount)
    {
        try
        {
            // Crear la compra en la base de datos
            var purchase = await _photoCreditService.CreatePurchaseAsync(userId, credits, amount);

            // Crear la sesión de pago en Stripe
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(amount * 100), // Stripe usa centavos
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Paquete de {credits} Créditos de Fotos",
                                    Description = $"Paquete de {credits} créditos para análisis de fotos con IA"
                                }
                            },
                            Quantity = 1
                        }
                    },
                Mode = "payment",
                SuccessUrl = $"{_configuration["App:BaseUrl"]}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_configuration["App:BaseUrl"]}/payment/cancel",
                ClientReferenceId = userId,
                Metadata = new Dictionary<string, string>
                    {
                        { "UserId", userId },
                        { "Credits", credits.ToString() },
                        { "PurchaseId", purchase.Id.ToString() }
                    }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // Actualizar la compra con el ID de sesión de Stripe
            purchase.StripeSessionId = session.Id;
            await _photoCreditService.UpdatePurchaseSessionIdAsync(purchase.Id, session.Id);

            return session.Url;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating Stripe checkout session: {ex.Message}");
            throw;
        }
    }

    public Task<PhotoCreditPackage[]> GetPhotoCreditPackagesAsync()
    {
        // Obtener los paquetes de créditos configurados
        var packages = new[]
        {
                new PhotoCreditPackage
                {
                    Credits = 10,
                    Price = 10.00m,
                    Description = "Paquete básico para pocas fotos",
                    IsMostPopular = false
                },
                new PhotoCreditPackage
                {
                    Credits = 50,
                    Price = 45.00m,
                    Description = "Paquete estándar con descuento del 10%",
                    IsMostPopular = true
                },
                new PhotoCreditPackage
                {
                    Credits = 200,
                    Price = 160.00m,
                    Description = "Paquete completo con descuento del 20%",
                    IsMostPopular = false
                }
            };

        return Task.FromResult(packages);
    }
}
