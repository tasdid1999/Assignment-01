using StudentCRUD.Dtos.User;

namespace StudentCRUD.Repository
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExistAsync(string email);

        Task<bool> IsUserExistAsync(string email, string password);

        Task<UserForToken> GetUserForTokenAsync(string email);
    }
}
