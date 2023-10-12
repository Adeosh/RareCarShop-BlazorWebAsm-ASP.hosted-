using RareCarShop.Shared;
using RareCarShop.Shared.Authentication;

namespace RareCarShop.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(RegisterModel request);
        Task<ServiceResponse<string>> Login(LoginModel request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePass request);
        Task<bool> IsUserAuthenticated();
    }
}
