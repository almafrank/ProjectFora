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
        Task<string> GetToken();
        Task<UserStatusDto> CheckUserLogin(string token);
        Task<string> ActivateUser(string token);
        Task<string> DeActivateUser(UserModel user);
        Task<UserModel> GetCurrentUser(string token);


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

                    if (token != null)
                    {
                        await _localStorageService.SetItemAsync("Token", token);

                        //Lägger till username i localstorage för att kunna hämta nuvarande användare
                        await _localStorageService.SetItemAsync("Username", userToRegister.Email);


                    }
                }
            }
        }

        //Fungerar
        public async Task Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("accounts/loginuser", loginModel);

            if (result.IsSuccessStatusCode)
            {
                await _localStorageService.RemoveItemAsync("Token");
                var token = await result.Content.ReadAsStringAsync();

                if (token != null)
                {
                    await _localStorageService.SetItemAsync("Token", token);

                    //Lägger till username i localstorage för att kunna hämta nuvarande användare
                    await _localStorageService.SetItemAsync("Username", loginModel.Email);

                    _navigationManager.NavigateTo("/interest");
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
            await _localStorageService.RemoveItemAsync("Username");

            _navigationManager.NavigateTo("/");
        }

        public async Task<string> GetToken()
        {
            // 1. Hämta token från Local Storage

            var token = await _localStorageService.GetItemAsStringAsync("Token");
            token = token.Replace("\"", "");

            return token;
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
                await _httpClient.PutAsJsonAsync($"accounts/edit/?accessToken={token}", user);
            }
        }

        public async Task<string> ActivateUser(string token)
        {
            var response = await _httpClient.GetAsync($"users/user?accessToken={token}");
            if (response.IsSuccessStatusCode)
            {
                return "You're account is now activated";
            }

            return "Something went wrong";
        }
        public async Task<string> DeActivateUser(UserModel user)
        {
            var response = await _httpClient.PutAsJsonAsync("accounts/deactivateuser", user);

            if (response.IsSuccessStatusCode)
            {
                return "You're account is now deactivated";
            }

            return "Something went wrong";
        }

        public async Task<UserModel> GetCurrentUser(string token)
        {
            var user = await _httpClient.GetFromJsonAsync<UserModel>($"api/users/user?accessToken={token}");
            if(user != null)
            {
                return user;
            }
            return null;
      
        }
    }
}
