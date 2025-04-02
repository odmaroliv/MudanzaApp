// MudanzaApp/Controllers/StripeWebhookController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudanzaApp.Data.Services;
using Stripe;

namespace MudanzaApp.Controllers
{
    [Route("api/stripe-webhook")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPhotoCreditService _photoCreditService;
        private readonly ILogger<StripeWebhookController> _logger;
        private readonly string _webhookSecret;

        public StripeWebhookController(
            IConfiguration configuration,
            IPhotoCreditService photoCreditService,
            ILogger<StripeWebhookController> logger)
        {
            _configuration = configuration;
            _photoCreditService = photoCreditService;
            _logger = logger;
            _webhookSecret = configuration["Stripe:WebhookSecret"] ?? "";
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                // Validar la firma del webhook de Stripe
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                // Manejar eventos específicos
                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted) // Cambiar Events a EventTypes
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    _logger.LogInformation($"Stripe checkout session completed: {session.Id}");
                    // Procesar la compra completada
                    await _photoCreditService.CompletePurchaseAsync(session.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded) // Cambiar Events a EventTypes
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation($"Payment intent succeeded: {paymentIntent.Id}");
                }
                else
                {
                    _logger.LogInformation($"Unhandled event type: {stripeEvent.Type}");
                }
                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError($"Stripe webhook error: {e.Message}");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected error processing webhook: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}