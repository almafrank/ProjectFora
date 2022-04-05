using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class UserService:IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RegisterResult> RegisterUser(RegisterModel register)
        {
            

            var respons= await _httpClient.PostAsJsonAsync("api/accounts",register);
            return await respons.Content.ReadFromJsonAsync<RegisterResult>();
        }
    }
}
