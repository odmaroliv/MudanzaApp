namespace MudanzaApp.Data.Services
{
    public interface IToastService
    {
        Task Success(string message, string title = null, int duration = 5000);
        Task Error(string message, string title = null, int duration = 5000);
        Task Warning(string message, string title = null, int duration = 5000);
        Task Info(string message, string title = null, int duration = 5000);
    }
}
