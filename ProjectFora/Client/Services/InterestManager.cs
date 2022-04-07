using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class InterestManager : IInterestManager
    {
        private readonly HttpClient _httpClient;
        public InterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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
            return await _httpClient.GetFromJsonAsync<List<InterestModel>>("interest/getallInterest");
        }

        public async Task PostAInterest(InterestModel postInterest)
        {
            await _httpClient.PostAsJsonAsync("interest/PostAInterest", postInterest);
        }

        public async Task UpdateInterest(int id, InterestModel interest)
        {
            var result = await _httpClient.PostAsJsonAsync<InterestModel>($"interest/updateInterest/{id}", interest);
            var updateInterest = await result.Content.ReadFromJsonAsync<List<InterestModel>>();
          
        }
    }

        
    
}
