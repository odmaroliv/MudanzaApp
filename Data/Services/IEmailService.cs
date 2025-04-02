namespace MudanzaApp.Data.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendMudanzaInvitationAsync(string email, string inviterName, string mudanzaName, string sharingCode);
        Task SendMudanzaStatusUpdateAsync(string email, string mudanzaName, string newStatus, string comments = null);
    }
}
