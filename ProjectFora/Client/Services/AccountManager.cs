using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using ProjectFora.Shared.AccountModels;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IAccountManager
    {
        Task RegisterUser(UserForRegistrationDto userForRegistration);
        Task Login(LoginModel loginModel);
        Task Logout();
        Task ChangePassword(RegisterModel user, string token);
        Task GetToken();

        Task<UserStatusDto> CheckUserLogin(string token);
    }
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;

        public AccountManager(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task RegisterUser(UserForRegistrationDto userForRegistration)
        {
            //Måste kolla så att inte användaren redan finns på databasen
            if(userForRegistration != null)
            {
                var result = await _httpClient.PostAsJsonAsync("accounts/registration", userForRegistration);
            }
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

        public async Task GetToken()
        {
            // 1. Hämta token från Local Storage

            var token = await _localStorageService.GetItemAsStringAsync("Token");
            token = token.Replace("\"", "");

            //StringContent stringContent = new StringContent();
            //string content = await _httpClient.GetStringAsync();
           
        }

        public async Task<UserStatusDto> CheckUserLogin(string token)
        {
            var response = await _httpClient.GetAsync($"accounts/check?accessToken={token}");

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<UserStatusDto>(result);

                return data;
            }

            return null;
        }

        public async Task ChangePassword(RegisterModel user, string token)
        {
            if(user != null)
            {
                await _httpClient.PostAsJsonAsync($"accounts/edit?accessToken={token}", user);
            }
        }

  
    }
}
