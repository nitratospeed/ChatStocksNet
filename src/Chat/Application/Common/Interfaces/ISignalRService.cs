using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISignalRService
    {
        Task SendMessage(string user, string message, string room, bool join);
    }
}
