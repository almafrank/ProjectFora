using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
    public interface IMessage
    {
        Task Post(MessageModel postmessage);
        Task<MessageModel?> DeleteMessage(int id);
        Task<List<MessageModel>> GetAllMessages();
    }

    public class Message : IMessage

    {
        private readonly HttpClient _httpClient;

        public Message(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //ta bort ett meddelande
        public async Task<MessageModel?> DeleteMessage(int id)
        {
            return await _httpClient.GetFromJsonAsync<MessageModel>($"message/deletemessage/{id}");
        }
        //hämta meddelanden
        public async Task<List<MessageModel>> GetAllMessages()
        {
            return await _httpClient.GetFromJsonAsync<List<MessageModel>>("message/GetMessages");
        }
        //ta bort ett meddelande
        public async Task Post(MessageModel postmessage)
        {
            await _httpClient.PostAsJsonAsync("message/postmessage", postmessage);
        }
    }
}
