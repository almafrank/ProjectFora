using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessageManager
    {
        Task CreateMessage(MessageDto message, string accessToken);
        Task DeleteMessage(int id, string token);
        Task<List<MessageModel>> GetThreadMessages(int id, string token);
        Task<string> UpdateAThreadMessage(int id, MessageDto updatedMessage, string accessToken);
    }

    public class MessageManager : IMessageManager

    {
        private readonly HttpClient _httpClient;

        public MessageManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Hämtar alla meddelanden
        public async Task<List<MessageModel>> GetAllThreadMessages(string token)
        {
            var result = await _httpClient.GetFromJsonAsync<List<MessageModel>>($"api/messages?accessToken={token}");
            if(result != null)
            {
                return result;
            }
            return null;
        }
        
        public async Task DeleteMessage(int id, string token)
        {
          var result = await _httpClient.DeleteAsync($"api/messages/{id}?token={token}");
        }

        public async Task<List<MessageModel>> GetThreadMessages(int id, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"api/messages/{id}?token={token}");
        }


        // Skapar ett meddelande i en tråd
        public async Task CreateMessage(MessageDto newMessage, string accessToken)
        {
           var result = await _httpClient.PostAsJsonAsync($"api/messages?accessToken={accessToken}", newMessage);
        }

        

        public async Task<string> UpdateAThreadMessage(int id, MessageDto updatedMessage, string accessToken)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/messages/{id}?accessToken={accessToken}", updatedMessage);
            if(result.IsSuccessStatusCode)
            {
                return "Message has been edited";
            }
            return null;
        }
    }
}
