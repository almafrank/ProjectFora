using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetCurrentUser()
        {
            await _httpClient.GetAsync("accounts/currentUser");
        }

        public async Task Login(UserForLoginDto loginModel)
        {
            await _httpClient.PostAsJsonAsync("accounts/login", loginModel);
        }

        public async Task Logout()
        {
            //await _httpClient.PostAsJsonAsync("accounts/logout");
        }

        public async Task RegisterUser(UserForRegistrationDto userForRegistration)
        {
            await _httpClient.PostAsJsonAsync("accounts/registration", userForRegistration);
        }
    }
}
