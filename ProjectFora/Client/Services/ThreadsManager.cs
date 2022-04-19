using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IThreadsManager
    {
        Task CreateThread(ThreadDto postThread, string accessToken);

        Task DeleteThread(int id, string accessToken);

        Task<List<ThreadModel>> GetAllThreads(string accessToken);

        Task<ThreadModel> GetThread(int id, string token);
        
        Task UpdateThread(int id, ThreadDto thread, string accessToken);

        
        //Task <List<ThreadModel>> SearchThread(string searchText);

    }
    public class ThreadsManager : IThreadsManager
    {
        private readonly HttpClient _httpClient;

        public ThreadsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteThread(int id, string accessToken)
        {
            await _httpClient.DeleteAsync($"api/threads/{id}?accessToken={accessToken}");
        }

        public async Task<List<ThreadModel>> GetAllThreads(string accessToken)
        {
            return await _httpClient.GetFromJsonAsync<List<ThreadModel>>($"api/threads?accessToken={accessToken}");
        }


        public async Task<ThreadModel> GetThread(int id, string token)
        {
            return await _httpClient.GetFromJsonAsync<ThreadModel>($"api/threads/thread{id}?token={token}");
        }

        public async Task CreateThread(ThreadDto postThread, string accessToken)
        {
           var result = await _httpClient.PostAsJsonAsync($"api/threads?accessToken={accessToken}", postThread);
        }

        public async Task UpdateThread(int id, ThreadDto threadToUpdate, string accessToken)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/threads/{id}?accessToken={accessToken}", threadToUpdate);
        }

        
        

        //public async Task<List<ThreadModel>> SearchThread(string searchText)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<ThreadModel>>
        //}
    }
}
