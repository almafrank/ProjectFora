using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessageManager
    {
        Task PostMessage(MessageModel postmessage);
        Task<MessageModel?> DeleteMessage(int id);
        Task<List<MessageModel>> GetAllMessages();
        Task<List<MessageModel>> GetThreadMessages(int threadId, string token);
    }

    public class MessageManager : IMessageManager

    {
        private readonly HttpClient _httpClient;

        public MessageManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Tar bort ett meddelande
        public async Task<MessageModel?> DeleteMessage(int id)
        {
            return await _httpClient.GetFromJsonAsync<MessageModel>($"message/deletemessage/{id}");
        }

        //Hämtar alla meddelanden
        public async Task<List<MessageModel>> GetAllMessages()
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>("message/GetMessages");
        }

        public async Task<List<MessageModel>> GetThreadMessages(int threadId, string token)
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>($"message/thread?id={threadId.ToString()}&token={token}");
        }

        //Lägger till ett meddelande
        public async Task PostMessage(MessageModel postmessage)
        {
            await _httpClient.PostAsJsonAsync("message/postmessage", postmessage);
        }
    }
}
