using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessageManager
    {
        Task PostAThreadMessage(MessageModel message,string token);
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
        //
        public async Task<List<MessageModel>> DeleteMessage(int threadId, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"message/thread?id={threadId.ToString()}&token={token}");
        }

       //Hämtar alla meddelanden
        public async Task<List<MessageModel>> GetAllThreadMessages()
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>("message/GetMessages");
        }

        public async Task<List<MessageModel>> GetThreadMessages(int threadId, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"message/thread?id={threadId.ToString()}&token={token}");
        }

        public Task<List<MessageModel>> GetThreadMessages()
        {
            throw new NotImplementedException();
        }

        public Task PostAThreadMessage(MessageModel message, string token)
        {
            throw new NotImplementedException();
        }

        //Lägger till ett meddelande
        public async Task PostMessage(MessageModel postmessage,string token)
        {
            await _httpClient.PostAsJsonAsync("message/postmessage", postmessage);
        }

        

        public Task<List<MessageModel>> UpdateAThreadMessage(int id, MessageModel updatedMessage, string token)
        {
            throw new NotImplementedException();
        }
    }
}
