

using ProjectFora.Shared;

namespace ProjectFora.Client.Services
{
    public interface IUserService
    {
        //
        Task<RegisterResult>RegisterUser (RegisterModel register);
    }
}
