using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IUserManager
    {
        Task Post(MessageModel postmessage);

    }
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Post(MessageModel message)
        {
           var result = await _httpClient.PostAsJsonAsync("api/message", message);
        }
    }
}
