
using Microsoft.JSInterop;
using MudanzaApp.Data.Services;

namespace MudanzaApp.Services
{


    public class ToastService : IToastService
    {
        private readonly IJSRuntime _jsRuntime;

        public ToastService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task Success(string message, string title = null, int duration = 5000)
        {
            await ShowToast("success", message, title, duration);
        }

        public async Task Error(string message, string title = null, int duration = 5000)
        {
            await ShowToast("error", message, title, duration);
        }

        public async Task Warning(string message, string title = null, int duration = 5000)
        {
            await ShowToast("warning", message, title, duration);
        }

        public async Task Info(string message, string title = null, int duration = 5000)
        {
            await ShowToast("info", message, title, duration);
        }

        private async Task ShowToast(string type, string message, string title, int duration)
        {
            await _jsRuntime.InvokeVoidAsync("showToast", type, message, title, duration);
        }
    }
}