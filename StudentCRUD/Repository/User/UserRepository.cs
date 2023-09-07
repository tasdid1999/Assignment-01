using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos.User;

namespace StudentCRUD.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserForToken> GetUserForTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            

            return new UserForToken(user, role[0]);
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);

            return user is not null ? true : false;

        }

        public async Task<bool> IsUserExistAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return false;
            }
            var isPasswordExist = await _userManager.CheckPasswordAsync(user, password);

            return user is not null && isPasswordExist ? true : false;

        }
        public async Task<bool> IsUserExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user is not null ? true : false;

        }
    }
}
