using ProjectFora.Shared;
using System.Net.Http.Json;

namespace ProjectFora.Client.Services
{
<<<<<<< Updated upstream
    public interface IMessage
    {

    }
    public class Message
=======
    public class Message : IMessage
>>>>>>> Stashed changes
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
