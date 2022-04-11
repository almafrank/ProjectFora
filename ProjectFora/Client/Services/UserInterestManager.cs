using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserInterestManager
    {
        Task PostUserInterest(UserInterestModel postinterest);
        Task<List<UserInterestModel>> GetUserInterest();
        Task DeleteUserInterest(int InterestId);
        //Task UpdateUserInterest(int InterestId);


    }
    public class UserInterestManager : IUserInterestManager
    {
        private readonly HttpClient _httpClient;

        public UserInterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }




        public async Task DeleteUserInterest(int InterestId)
        {
            await _httpClient.GetFromJsonAsync<UserInterestModel>($"UserInterest/DeleteUserInterest/{InterestId}");
        }

        public async Task<List<UserInterestModel>> GetUserInterest()
        {
            return await _httpClient.GetFromJsonAsync<List<UserInterestModel>>("UserInterest/GetAllUserInterest");
        }

        public async Task PostUserInterest(UserInterestModel postinterest)
        {
            await _httpClient.PostAsJsonAsync("UserInterest/UserPostnewInterest", postinterest);
        }

        //public async Task<List<UserInterestModel>> UpdateUserInterest(int InterestId)
        //{
        //    var result = await _httpClient.PostAsJsonAsync<UserInterestModel>($"UserInterest/UpdateUserInterest/{InterestId}");
        //    var updateUserInterest = await result.Content.ReadFromJsonAsync<List<UserInterestModel>>();
        //    return updateUserInterest;
        //}
    }
}
