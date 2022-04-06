namespace ProjectFora.Client.Services
{
    public interface IUserManager
    {
        Task RegisterUser(UserForRegistrationDto userForRegistration);
        Task Login(UserForLoginDto loginModel);
        Task Logout();
        Task GetCurrentUser();
    }
}
