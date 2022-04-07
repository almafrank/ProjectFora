using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        public UserModel User { get; private set; }

        public AccountManager(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task RegisterUser(UserForRegistrationDto userForRegistration)
        {
            await _httpClient.PostAsJsonAsync("accounts/registration", userForRegistration);
        }

        public async Task Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("accounts/login", loginModel);
                var token = await result.Content.ReadAsStringAsync();

            if (token != null)
            {
                await _localStorageService.SetItemAsync("Token", token);
                _navigationManager.NavigateTo("/");
            }
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("Token");
            _navigationManager.NavigateTo("/");
  
        }

       
        public async Task GetCurrentUser()
        {
            await _httpClient.GetAsync("accounts/currentUser");
        }
    }
}
