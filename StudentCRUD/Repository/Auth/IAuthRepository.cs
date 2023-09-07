using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos;

namespace StudentCRUD.Repository
{
    public interface IAuthRepository
    {
        Task<bool> LoginAsync(UserLoginRequest user);

        Task<bool> RegisterAsync(UserRegisterRequest user);




    }
}
