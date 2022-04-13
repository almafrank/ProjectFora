using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IThreadsManager
    {
        //
        Task PostThread(ThreadModel postThread);

        Task<ThreadModel> DeleteThread(int id);

        Task<List<ThreadModel>> GetAllThreads();

        Task<ThreadModel> GetThread(int id);
        
        Task UpdateThread(int id, ThreadModel thread);

    }
    public class ThreadsManager : IThreadsManager
    {
        private readonly HttpClient _httpClient;

        public ThreadsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ThreadModel> DeleteThread(int id)
        {
            return await _httpClient.GetFromJsonAsync<ThreadModel>($"Threads/DeleteThread/{id}");
        }

        public async Task<List<ThreadModel>> GetAllThreads()
        {
            return await _httpClient.GetFromJsonAsync<List<ThreadModel>>("Threads/AllThreads");
        }

        public async Task<ThreadModel> GetThread(int id)
        {
            return await _httpClient.GetFromJsonAsync<ThreadModel>($"Threads/GetAThread/{id}");
        }

        public async Task PostThread(ThreadModel postThread)
        {
            await _httpClient.PostAsJsonAsync("Threads/PostThead", postThread);
        }

        public async Task UpdateThread(int id, ThreadModel thread)
        {
            var result = await _httpClient.PostAsJsonAsync($"Threads/updateThread{id}", thread);
        }
    }
}
