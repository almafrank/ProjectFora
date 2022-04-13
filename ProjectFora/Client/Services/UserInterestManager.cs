using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserInterestManager
    {
        Task AddUserInterest(InterestModel interest, string token);
        Task<List<InterestModel>> GetUserInterests(string token);
        //Task DeleteUserInterest(int InterestId);
        //Task UpdateUserInterest(int InterestId);
        //Task<UserInterestModel> GetSingelInterest(int InterestId);

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
                int x = 1;
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

        public async Task DeleteUserInterest(int Id, string token)
        {
            await _httpClient.GetFromJsonAsync<UserInterestModel>($"api/UserInterest/DeleteUserInterest?accessToken={token}/{Id}");
        }

        //public async Task<UserInterestModel> GetSingelInterest(int InterestId)
        //{
        //    return await _httpClient.GetFromJsonAsync<UserInterestModel>($"UserInterest/GetUserSingelInterest/{InterestId}");
        //}




        //public Task UpdateUserInterest(int InterestId)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task UpdateUserInterest(int InterestId)
        //{
        //    var result = await _httpClient.PutAsync($"UserInterest/UpdateUserInterest{InterestId}");
        //    var updateUserInterest = await result.Content.ReadFromJsonAsync();
        //    return updateUserInterest;
        //}


    }
}
