using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos.Auth;
using StudentCRUD.Dtos.User;

namespace StudentCRUD.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> ForgotPassword(string email);
        LoginResponse GetJwtToken(UserForToken user);

    }
}
