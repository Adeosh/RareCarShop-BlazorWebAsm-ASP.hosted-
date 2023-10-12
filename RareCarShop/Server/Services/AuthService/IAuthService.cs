using RareCarShop.Shared;
using RareCarShop.Shared.Authentication;

namespace RareCarShop.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserModel user, string password);
        Task<bool> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        int GetUserId();
        string GetUserEmail();
        Task<UserModel> GetUserByEmail(string email);
    }
}
