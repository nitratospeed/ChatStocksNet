using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AuthUser(string email, string password);
        Task<bool> RegisterUser(string email, string password);
    }
}
