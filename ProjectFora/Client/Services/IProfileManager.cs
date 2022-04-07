

namespace ProjectFora.Client.Services
{
    public interface IProfileManager
    {
        Task<List<UserModel>>GetAllUsers();
        Task PostANewUser(UserModel userToAdd);
        Task<UserModel> GetUser(int id);
        Task<List<UserModel>> UpdateUser(UserModel user, int id);


    }
}
