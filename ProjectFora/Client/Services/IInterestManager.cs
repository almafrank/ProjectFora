namespace ProjectFora.Client.Services
{
    public interface IInterestManager
    {
        Task<List<InterestModel>> GetAllInterest();
        Task<InterestModel> GetAInterest(int id);
        Task PostAInterest(InterestModel postInterest);
        Task UpdateInterest(int id, InterestModel interest);
        Task<InterestModel> DeleteInterest(int id);
    }
}
