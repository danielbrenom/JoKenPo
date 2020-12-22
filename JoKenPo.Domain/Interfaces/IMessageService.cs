using System.Threading.Tasks;

namespace JoKenPo.Domain.Interfaces
{
    public interface IMessageService
    {
        public Task ShowError(string message);
        public Task ShowAlert(string message);
        public Task ShowSuccess(string message);
    }
}