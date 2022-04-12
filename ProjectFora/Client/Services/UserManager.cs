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
        Task<UserModel> GetCurrentUser();
        Task<UserStatusDto> CheckUserLogin(string token);
        Task<string> ActivateUser(UserModel user);
        Task<string> DeActivateUser(UserModel user);
        Task<List<UserInterestModel>> GetUserInterests();

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
        public async Task RegisterUser(RegisterModel userForRegistration)
        {
            //Måste kolla så att inte användaren redan finns på databasen
            if (userForRegistration != null)
            {
                await _httpClient.PostAsJsonAsync("accounts/registration", userForRegistration);
            }
        }

        //Fungerar
        public async Task Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("accounts/loginuser", loginModel);
            if(result.IsSuccessStatusCode)
            {
                await _localStorageService.RemoveItemAsync("Token");
                var token = await result.Content.ReadAsStringAsync();

                if (token != null)
                {
                    await _localStorageService.SetItemAsync("Token", token);

                    //Lägger till username i localstorage för att kunna hämta nuvarande användare
                    await _localStorageService.SetItemAsync("Username", loginModel.Email);

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
            var response = await _httpClient.GetAsync($"accounts/check?accessToken={token}");

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

        //Fungerar
        public async Task<UserModel> GetCurrentUser()
        {
            var email = _localStorageService.GetItemAsStringAsync("Username");

            if (email != null)
            {
                var response = await _httpClient.GetAsync($"interest/currentuser?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<UserModel>(result);

                    return data;
                }
            }
            return null;
        }

        public async Task<string> ActivateUser(UserModel user)
        {
            var response = await _httpClient.PutAsJsonAsync("accounts/activateuser", user);
            if(response.IsSuccessStatusCode)
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

        public async Task<List<UserInterestModel>> GetUserInterests()
        {
            var result = await _httpClient.GetFromJsonAsync<List<UserInterestModel>>("user/getinterests");
            if(result != null)
            {
                return result;
            }
            return null;
        }
    }
}
