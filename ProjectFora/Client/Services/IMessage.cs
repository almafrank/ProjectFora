namespace ProjectFora.Client.Services
{
    public interface IMessage
    {
        Task Post(MessageModel postmessage);
        Task<MessageModel?> DeleteMessage(int id);
        Task<List<MessageModel>> GetAllMessages();
    }
}
