using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserManager
    {
        Task RegisterUser(RegisterModel registerUser);
        Task Login(LoginModel loginModel);
        Task Logout();
        Task ChangePassword(EditPasswordModel user, string token);
        Task<UserStatusDto> CheckUserLogin(string token);
        Task<string> SetUserStatus(string token);
        Task<UserModel> GetCurrentUser(string token);
        Task<string> GetToken();


    }

    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;

        public UserManager(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        //Fungerar
        public async Task RegisterUser(RegisterModel userToRegister)
        {
            if (userToRegister != null)
            {
                var result = await _httpClient.PostAsJsonAsync("api/users", userToRegister);
                if (result.IsSuccessStatusCode)
                {
                    await _localStorageService.RemoveItemAsync("Token");
                    var token = await result.Content.ReadAsStringAsync();

                    // "Loggar in"
                    if (token != null)
                    {
                        await _localStorageService.SetItemAsync("Token", token);

                    }
                }
            }
        }


        public async Task Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/users/loginUser", loginModel);

            if (result.IsSuccessStatusCode)
            {
                await _localStorageService.RemoveItemAsync("Token");
                var token = await result.Content.ReadAsStringAsync();

                if (token != null)
                {
                    await _localStorageService.SetItemAsync("Token", token);

                    _navigationManager.NavigateTo("/profile");
                }
                else
                {
                    _navigationManager.NavigateTo("/login");
                }
            }
        }

        public async Task Logout()
        {
            //Loggar ut genom att ta bort tokens från localstorage

            await _localStorageService.RemoveItemAsync("Token");

            _navigationManager.NavigateTo("/");
        }


        public async Task<UserStatusDto> CheckUserLogin(string token)
        {
            var response = await _httpClient.GetAsync($"api/users/check?accessToken={token}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<UserStatusDto>(result);

                return data;
            }
            return null;
        }

        public async Task ChangePassword(EditPasswordModel user, string token)
        {
            if (user != null)
            {
                await _httpClient.PutAsJsonAsync($"api/users?accessToken={token}", user);
            }
        }

        public async Task<string> SetUserStatus(string token)
        {
            var response = await _httpClient.GetAsync($"users/userStatus?accessToken={token}");

            return response.Content.ToString();

        }

        public async Task<UserModel> GetCurrentUser(string token)
        {
            var user = await _httpClient.GetFromJsonAsync<UserModel>($"api/users/user?accessToken={token}");
            if (user != null)
            {
                return user;
            }
            return null;

        }

        public async Task<string> GetToken()
        {
            var token = await _localStorageService.GetItemAsStringAsync("Token");
            token = token.Replace("\"", "");
            if(token != null)
            {
                return token;

            }
            return null;

        }
    }
}
