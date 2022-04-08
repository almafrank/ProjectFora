using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IProfileManager
    {
        Task<List<AccountUserModel>> GetAllUsers();
        Task PostANewUser(AccountUserModel userToAdd);
        Task<AccountUserModel> GetUser(int id);
        Task<List<AccountUserModel>> UpdateUser(AccountUserModel user, int id);
        Task<AccountUserModel> DeleteUser(int id);


    }
    public class ProfileManager : IProfileManager
    {
        private readonly HttpClient _httpClient;

        public ProfileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AccountUserModel>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<AccountUserModel>>("profile/Users");
        }
        public async Task PostANewUser(AccountUserModel userToAdd)
        {
            await _httpClient.PostAsJsonAsync("profile/newUser", userToAdd);
        }
        public async Task<AccountUserModel> GetUser(int id)
        {
            return await _httpClient.GetFromJsonAsync<AccountUserModel>($"profile/user/{id}");


        }
        public async Task<List<AccountUserModel>>UpdateUser(AccountUserModel user,int id)
        {
            var result = await _httpClient.PostAsJsonAsync<AccountUserModel>($"profile/update{id}",user);
            var updateUser = await result.Content.ReadFromJsonAsync<List<AccountUserModel>>();
            return updateUser;
        }

        public async Task<AccountUserModel> DeleteUser(int id)
        {
            return await _httpClient.GetFromJsonAsync<AccountUserModel>($"profile/deleteUser/{id}");
        }
    }
}

