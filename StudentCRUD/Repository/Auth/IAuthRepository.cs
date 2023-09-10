using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos;
using StudentCRUD.Dtos.Auth;

namespace StudentCRUD.Repository
{
    public interface IAuthRepository
    {
        Task<bool> LoginAsync(UserLoginRequest user);

        Task<bool> RegisterAsync(UserRegisterRequest user);

        Task<bool> ResetPassword(ResetPasswordRequest resetPassword);



    }
}
