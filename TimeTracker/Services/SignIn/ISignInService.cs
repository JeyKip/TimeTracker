using System.Net.Http;
using System.Threading.Tasks;
using TimeTracker.Services.Storage.Entities.SignIn;

namespace TimeTracker.Services.SignIn
{
    public interface ISignInService
    {
        bool IsAuthorized { get; }
        string UserDisplayName { get; }

        Task<SignInResult> SignInAsync();
        Task SignOutAsync();
        Task RefreshTokenAsync();
        SigninStore GetStatus();
    }
}