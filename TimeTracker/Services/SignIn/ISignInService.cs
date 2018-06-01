using System.Threading.Tasks;

namespace TimeTracker.Services.SignIn
{
    public interface ISignInService
    {
        string AccessToken { get; set; }
        bool IsAuthorized { get; }
        string RefreshToken { get; set; }
        string UserDisplayName { get; set; }

        Task<SignInResult> SignInAsync();
        Task SignOutAsync();
    }
}