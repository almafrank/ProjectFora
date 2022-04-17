using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessageManager
    {
        Task CreateMessage(int Id, MessageModel message, string token);
        Task<List<MessageModel>> DeleteMessage(int threadId,string token);
        Task<List<MessageModel>> GetThreadMessages();
        Task<List<MessageModel>> GetThreadMessages(int threadId, string token);
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

        public async Task<List<MessageModel>> GetThreadMessages(int threadId, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"message/thread/{threadId}?token={token}");
        }

        public Task<List<MessageModel>> GetThreadMessages()
        {
            throw new NotImplementedException();
        }

        public Task PostAThreadMessage(MessageModel message, string token)
        {
            throw new NotImplementedException();
        }

        // Skapar ett meddelande i en tråd
        public async Task CreateMessage(int Id, MessageModel newMessage,string token)
        {
            await _httpClient.PostAsJsonAsync($"api/messages?id={Id}&accessToken={token}", newMessage);
        }

        

        public Task<List<MessageModel>> UpdateAThreadMessage(int id, MessageModel updatedMessage, string token)
        {
            throw new NotImplementedException();
        }
    }
}
