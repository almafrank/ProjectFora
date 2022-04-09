namespace ProjectFora.Client.Services
{
    public interface IUserInterestManager
    {
        Task PostUserInterest(UserInterestModel postinterest);
        Task<List<UserInterestModel>> GetUserInterest();


    }
    public class UserInterestManager : IUserInterestManager
    {
        public Task<List<UserInterestModel>> GetUserInterest()
        {
            throw new NotImplementedException();
        }

        public Task PostUserInterest(UserInterestModel postinterest)
        {
            throw new NotImplementedException();
        }
    }
}
