using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos;

namespace StudentCRUD.Repository
{
    public interface IAuthRepository
    {
        Task<bool> LoginAsync(UserLoginRequest user);

        Task<bool> IsEmailExistAsync(string email);

        Task<bool> IsUserExistAsync(string email , string password);    

        Task<bool> RegisterAsync(UserRegisterRequest user);


    }
}
