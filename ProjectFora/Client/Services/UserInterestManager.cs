using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserInterestManager
    {
        Task AddUserInterest(InterestModel interest, string token);
        Task<List<InterestModel>> GetUserInterests(string token);
        Task<string> DeleteUserInterest(int Id, string token);
        Task<string> EditInterest(int Id, string token);

    }
    public class UserInterestManager : IUserInterestManager
    {
        private readonly HttpClient _httpClient;

        public UserInterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InterestModel>> GetUserInterests(string token)
        {
            var result = await _httpClient.GetFromJsonAsync<List<InterestModel>>($"api/userinterest?accessToken={token}");
            if (result != null)
            {
                return result;
           
            }
            return null;
        }
        public async Task AddUserInterest(InterestModel interest, string token)
        {

            if (interest != null)
            {
                await _httpClient.PostAsJsonAsync($"api/userinterest?accessToken={token}", interest);
            }
        }

        public async Task<string> DeleteUserInterest(int Id, string token)
        {
            var result = await _httpClient.DeleteAsync($"api/UserInterest/{Id}?accessToken={token}");
            return result.ToString();
        }

        public async Task<string> EditInterest(int Id, string editedName, string token)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/UserInterest/{Id}?accessToken={token}", editedName);
            return result.ToString();
        }

        public Task<string> EditInterest(int Id, string token)
        {
            throw new NotImplementedException();
        }
    }
}
