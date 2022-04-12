using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserInterestManager
    {
        Task AddUserInterest(UserInterestModel user);
        //Task<List<UserInterestModel>> GetUserInterest();
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

        public async Task AddUserInterest(UserInterestModel user)
        {
            if(user != null)
            {
                await _httpClient.PostAsJsonAsync("UserInterest/postUserInterest", user);
            }
           
        }


        //public async Task DeleteUserInterest(int InterestId)
        //{
        //    await _httpClient.GetFromJsonAsync<UserInterestModel>($"UserInterest/DeleteUserInterest/{InterestId}");
        //}

        //public async Task<UserInterestModel> GetSingelInterest(int InterestId)
        //{
        //    return await _httpClient.GetFromJsonAsync<UserInterestModel>($"UserInterest/GetUserSingelInterest/{InterestId}");
        //}

        public async Task<List<UserInterestModel>> GetUserInterest()
        {
            return await _httpClient.GetFromJsonAsync<List<UserInterestModel>>("UserInterest/GetAllUserInterest");
        }



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
