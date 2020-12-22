using System.Threading.Tasks;
using JoKenPo.Domain.Interfaces;
using Microsoft.JSInterop;

namespace JoKenPo.Services
{
    public class MessageService : IMessageService
    {
        public MessageService(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        private IJSRuntime JsRuntime { get; }

        public async Task ShowError(string message)
        {
            await JsRuntime.InvokeVoidAsync("showAlert", "Error", "error", message);
        }

        public async Task ShowAlert(string message)
        {
            await JsRuntime.InvokeVoidAsync("showAlert", "Atention", "alert", message);
        }

        public async Task ShowSuccess(string message)
        {
            await JsRuntime.InvokeVoidAsync("showAlert", "Success", "success", message);
        }
    }
}