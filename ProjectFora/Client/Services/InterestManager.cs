﻿using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IInterestManager
    {
        Task<List<InterestModel>> GetAllInterest(string token);
        Task<InterestModel> GetInterest(int id, string token);
        Task CreateInterest(InterestModel postInterest, string token);
        Task UpdateInterest(InterestModel updateInterest, string token);
        Task<string> DeleteInterest(int id, string token);
    }
    public class InterestManager : IInterestManager
    {
        private readonly HttpClient _httpClient;

        public InterestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<InterestModel>> GetAllInterest(string token)
        {
            var result = await _httpClient.GetFromJsonAsync<List<InterestModel>>($"api/interests?accessToken={token}");

            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task<InterestModel> GetInterest(int id, string token)
        {
           var interest = await _httpClient.GetFromJsonAsync<InterestModel>($"api/interests/{id}?accessToken={token}");

            if(interest != null)
            {
                return interest;
            }

            return null;
        }

        public async Task CreateInterest(InterestModel postInterest, string token)
        {
            await _httpClient.PostAsJsonAsync($"interests?accessToken={token}", postInterest);
        }

        public async Task UpdateInterest(InterestModel updateInterest, string token)
        {
            await _httpClient.PostAsJsonAsync($"interests?accessToken={token}", updateInterest);
        }

        public async Task<string> DeleteInterest(int id, string token)
        {
            var result = await _httpClient.DeleteAsync($"interest/{id}?accessToken={token}");
            if (result.IsSuccessStatusCode)
            {
                return "Interest deleted";
            }

            return "Interest couldn't be deleted";
        }
    }
}
