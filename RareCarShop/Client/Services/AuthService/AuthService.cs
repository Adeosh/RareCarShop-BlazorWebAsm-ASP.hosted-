using Microsoft.AspNetCore.Components.Authorization;
using RareCarShop.Shared;
using RareCarShop.Shared.Authentication;
using System.Net.Http.Json;

namespace RareCarShop.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePass request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/change-password", request.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<ServiceResponse<string>> Login(LoginModel request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(RegisterModel request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
