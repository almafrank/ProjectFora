using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessageManager
    {
        Task CreateMessage(MessageDto message, string accessToken);
        Task<List<MessageModel>> DeleteMessage(int threadId,string token);
        Task<List<MessageModel>> GetThreadMessages(int id, string token);
        Task<List<MessageModel>> UpdateAThreadMessage(int id, MessageModel updatedMessage, string token);
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
        
        public async Task<List<MessageModel>> DeleteMessage(int threadId, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"message/thread?id={threadId.ToString()}&token={token}");
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

        

        public Task<List<MessageModel>> UpdateAThreadMessage(int id, MessageModel updatedMessage, string token)
        {
            throw new NotImplementedException();
        }
    }
}
