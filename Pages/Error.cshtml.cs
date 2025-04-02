using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace MudanzaApp.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;
        private readonly IConfiguration _configuration;

        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public bool ShowDetails { get; set; }
        public string AppName { get; set; }

        public ErrorModel(ILogger<ErrorModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            AppName = _configuration["App:Name"] ?? "MudanzaApp";
        }

        public void OnGet(int? statusCode = null)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            StatusCode = statusCode ?? (exception != null ? 500 : 404);

            ErrorMessage = StatusCode switch
            {
                404 => "Page Not Found",
                500 => "Internal Server Error",
                401 => "Unauthorized",
                403 => "Forbidden",
                _ => "An error occurred"
            };

            if (exception != null)
            {
                ErrorDetails = exception.Message;
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
            }

            // Solo mostrar detalles del error en desarrollo
            ShowDetails = HttpContext.Request.Host.Value.Contains("localhost") ||
                          HttpContext.Request.Host.Value.Contains("127.0.0.1");
        }
    }
}