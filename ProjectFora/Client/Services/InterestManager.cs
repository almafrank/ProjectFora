using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IInterestManager
    {
        Task<List<InterestModel>> GetAllInterest();
        Task<InterestModel> GetAInterest(int id);
        Task PostAInterest(InterestModel postInterest);
        Task<List<InterestModel>> UpdateInterest(int id, InterestModel interest);
        Task<InterestModel> DeleteInterest(int id);
        Task SetUser(AccountUserModel user);
        Task<AccountUserModel> CurrentUser(string email);
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
        public AccountUserModel User { get; set; } = new();

        public async Task<InterestModel> DeleteInterest(int id)
        {
            return await _httpClient.GetFromJsonAsync<InterestModel>($"interest/deleteInterest/{id}");
        }

        public async Task<InterestModel> GetAInterest(int id)
        {
            return await _httpClient.GetFromJsonAsync<InterestModel>($"interest/GetAInterest/{id}");
        }

        public async Task<List<InterestModel>> GetAllInterest()
        {
            var result = await _httpClient.GetFromJsonAsync<List<InterestModel>>("interest/getallInterest");
            if(result != null)
            {
                return result;
            }
            return null;
        }
        public async Task SetUser(AccountUserModel userForRegistration)
        {
            if (userForRegistration != null)
            {
                var response = await _httpClient.PostAsJsonAsync<AccountUserModel>("Interest", userForRegistration);

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
        public async Task<AccountUserModel> CurrentUser(string email)
        {
            if (email != null)
            {
                var response = await _httpClient.GetAsync($"interest/currentuser?email={email}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<AccountUserModel>(result);

                    return data;
                }
            }
            return null;
        }
    }



}
