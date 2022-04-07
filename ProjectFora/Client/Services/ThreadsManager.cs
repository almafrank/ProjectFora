using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public class ThreadsManager:IThreadsManager
    {
        private readonly HttpClient _httpClient;
      
        public ThreadsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ThreadModel> DeleteThread(int id)
        {
            return await _httpClient.GetFromJsonAsync<ThreadModel>($"threads/DeleteThread/{id}");
        }

        public async Task<List<ThreadModel>> GetAllThreads()
        {
            return await _httpClient.GetFromJsonAsync<List<ThreadModel>>("threads/AllThreads");
        }

        public async Task<ThreadModel> GetThread(int id)
        {
            return await _httpClient.GetFromJsonAsync<ThreadModel>($"threads/GetAThread/{id}");
        }

        public async Task PostThread(ThreadModel postThread)
        {
            await _httpClient.PostAsJsonAsync("threads/PostThead", postThread);
        }

        //public async Task<List<ThreadModel>> UpdateThread(int id, ThreadModel thread)
        //{
        //    var result = await _httpClient.PostAsJsonAsync<ThreadModel>($"threads/updatethread{id}",thread);
        //    var updateThread = await result.Content.ReadFromJsonAsync<List<ThreadModel>>();
        //    return updateThread;
        //}
    }
}
