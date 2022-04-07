using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class ProfileManager : IProfileManager
    {
        private readonly HttpClient _httpClient;

        public ProfileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>("profile/Users");
        }
        public async Task PostANewUser(UserModel userToAdd)
        {
            await _httpClient.PostAsJsonAsync("profile/newUser", userToAdd);
        }
        public async Task<UserModel> GetUser(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserModel>($"profile/user/{id}");


        }
        public async Task<List<UserModel>>UpdateUser(UserModel user,int id)
        {
            var result = await _httpClient.PostAsJsonAsync<UserModel>($"profile/update{id}",user);
            var updateUser = await result.Content.ReadFromJsonAsync<List<UserModel>>();
            return updateUser;
        }

        public async Task<UserModel> DeleteUser(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserModel>($"profile/deleteUser/{id}");
        }
    }
}

