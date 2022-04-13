using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IInterestManager
    {
        Task<List<InterestModel>> GetAllInterest();
        Task<InterestModel> GetInterest(int id);
        Task PostAInterest(InterestModel postInterest);
        Task<List<InterestModel>> UpdateInterest(int id, InterestModel interest);
        Task<InterestModel> DeleteInterest(int id);
        Task SetUser(UserModel user);
        Task<UserModel> CurrentUser(string email);
    }
    public class InterestManager : IInterestManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public InterestManager(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public UserModel User { get; set; } = new();

        public async Task<InterestModel> DeleteInterest(int id)
        {
            return await _httpClient.GetFromJsonAsync<InterestModel>($"interest/deleteInterest/{id}");
        }

        public async Task<InterestModel> GetInterest(int id)
        {
           var interest = await _httpClient.GetFromJsonAsync<InterestModel>($"api/interests/{id}");
            if(interest != null)
            {
                return interest;
            }
            return null;
        }

        public async Task<List<InterestModel>> GetAllInterest()
        {
            var result = await _httpClient.GetFromJsonAsync<List<InterestModel>>("api/interests");
            if(result != null)
            {
                return result;
            }
            return null;
        }
        public async Task SetUser(UserModel userForRegistration)
        {
            if (userForRegistration != null)
            {
                var response = await _httpClient.PostAsJsonAsync<UserModel>("Interest", userForRegistration);

            }
        }
        public async Task PostAInterest(InterestModel postInterest)
        {
            await _httpClient.PostAsJsonAsync("interest/PostAInterest", postInterest);
        }

        public async Task<List<InterestModel>?> UpdateInterest(int id, InterestModel interest)
        {
            var result = await _httpClient.PostAsJsonAsync<InterestModel>($"interest/updateInterest/{id}", interest);
            var updateInterest = await result.Content.ReadFromJsonAsync<List<InterestModel>>();
            return updateInterest;

        }
        public async Task<UserModel> CurrentUser(string email)
        {
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
    }



}
