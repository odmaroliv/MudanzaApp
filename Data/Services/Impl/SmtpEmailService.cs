using MudanzaApp.Data.Services;
using System.Net;
using System.Net.Mail;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly string _senderName;
    private readonly string _baseUrl;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _smtpServer = _configuration["Email:SmtpServer"];
        _smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        _smtpUsername = _configuration["Email:Username"];
        _smtpPassword = _configuration["Email:Password"];
        _senderEmail = _configuration["Email:SenderEmail"];
        _senderName = _configuration["Email:SenderName"];
        _baseUrl = _configuration["App:BaseUrl"];
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email));

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

                await client.SendMailAsync(message);
            }

            _logger.LogInformation($"Email sent to {email} with subject '{subject}'");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email to {email}: {ex.Message}");
            throw;
        }
    }

    public async Task SendMudanzaInvitationAsync(string email, string inviterName, string mudanzaName, string sharingCode)
    {
        var subject = $"Te han invitado a colaborar en una mudanza: {mudanzaName}";

        var collaborationUrl = $"{_baseUrl}/mudanza/collaborate/{sharingCode}";

        var htmlMessage = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4a6da7; color: white; padding: 10px; text-align: center; }}
                        .content {{ padding: 20px; border: 1px solid #ddd; }}
                        .button {{ display: inline-block; background-color: #4a6da7; color: white; padding: 10px 20px; 
                                  text-decoration: none; border-radius: 5px; margin-top: 20px; }}
                        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Invitación para Colaborar en Mudanza</h2>
                        </div>
                        <div class='content'>
                            <p>Hola,</p>
                            <p><strong>{inviterName}</strong> te ha invitado a colaborar en la mudanza <strong>{mudanzaName}</strong>.</p>
                            <p>Puedes agregar tus artículos y ayudar a organizar esta mudanza haciendo clic en el botón a continuación:</p>
                            <p style='text-align: center;'>
                                <a href='{collaborationUrl}' class='button'>Colaborar en la Mudanza</a>
                            </p>
                            <p>O copia y pega el siguiente enlace en tu navegador:</p>
                            <p>{collaborationUrl}</p>
                        </div>
                        <div class='footer'>
                            <p>Este es un mensaje automático, por favor no respondas a este correo.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

        await SendEmailAsync(email, subject, htmlMessage);
    }

    public async Task SendMudanzaStatusUpdateAsync(string email, string mudanzaName, string newStatus, string comments = null)
    {
        var subject = $"Actualización de estado de mudanza: {mudanzaName}";

        var statusText = GetStatusDisplayText(newStatus);
        var mudanzaUrl = $"{_baseUrl}/mudanza/details/{mudanzaName}";

        var htmlMessage = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4a6da7; color: white; padding: 10px; text-align: center; }}
                        .content {{ padding: 20px; border: 1px solid #ddd; }}
                        .status {{ font-weight: bold; color: #4a6da7; }}
                        .button {{ display: inline-block; background-color: #4a6da7; color: white; padding: 10px 20px; 
                                  text-decoration: none; border-radius: 5px; margin-top: 20px; }}
                        .footer {{ margin-top: 20px; font-size: 12px; color: #777; text-align: center; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Actualización de Estado de Mudanza</h2>
                        </div>
                        <div class='content'>
                            <p>Hola,</p>
                            <p>El estado de tu mudanza <strong>{mudanzaName}</strong> ha sido actualizado a <span class='status'>{statusText}</span>.</p>
                            {(string.IsNullOrEmpty(comments) ? "" : $"<p><strong>Comentarios:</strong> {comments}</p>")}
                            <p>Puedes ver los detalles de tu mudanza haciendo clic en el botón a continuación:</p>
                            <p style='text-align: center;'>
                                <a href='{mudanzaUrl}' class='button'>Ver Detalles</a>
                            </p>
                        </div>
                        <div class='footer'>
                            <p>Este es un mensaje automático, por favor no respondas a este correo.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

        await SendEmailAsync(email, subject, htmlMessage);
    }

    // Método auxiliar para obtener el texto de estado para mostrar al usuario
    private string GetStatusDisplayText(string status)
    {
        return status switch
        {
            "DraftOpen" => "Borrador Abierto",
            "DraftClosed" => "Borrador Cerrado",
            "InReview" => "En Revisión",
            "WaitingForDocuments" => "En Espera de Documentos",
            "ReceivedInWarehouseUS" => "Recibido en Almacén USA",
            "ReceivedInWarehouseMX" => "Recibido en Almacén México",
            "WaitingForCrossing" => "En Espera para Cruce",
            "WaitingForShipment" => "En Espera para Envío",
            "InTransit" => "En Tránsito",
            "Delivered" => "Entregado",
            "Completed" => "Completado",
            _ => status
        };
    }
}
