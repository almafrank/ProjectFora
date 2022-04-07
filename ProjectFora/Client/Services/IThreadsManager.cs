namespace ProjectFora.Client.Services
{
    public interface IThreadsManager
    {
        Task PostThread(ThreadModel postThread);
        Task <ThreadModel>DeleteThread(int id);
        Task<List<ThreadModel>> GetAllThreads();
        Task<ThreadModel> GetThread(int id);
        //Task<List<ThreadModel>> UpdateThread(int id, ThreadModel thread);

    }
}
