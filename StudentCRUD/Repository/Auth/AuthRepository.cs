using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.DataAccess;
using StudentCRUD.Dtos;

namespace StudentCRUD.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public AuthRepository(UserManager<IdentityUser> userManager, IMapper mapper,SignInManager<IdentityUser> signInManager,ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;

        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            
            var user = await _userManager.FindByEmailAsync(email);

           return user is not null ?  true : false;
         
        }

        public async Task<bool> IsUserExistAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                return false;
            }
            var isPasswordExist = await _userManager.CheckPasswordAsync(user, password);
            
            return user is not null && isPasswordExist ? true : false;

        }

        public async Task<bool> LoginAsync(UserLoginRequest user)
        {

            var dbActionResult = await _signInManager.PasswordSignInAsync(user.Email,user.Password,false,false);

            return dbActionResult.Succeeded ? true : false;
        }

        public async Task<bool> RegisterAsync(UserRegisterRequest user)
        {
            var identityUser = new IdentityUser()
            {
                UserName = user.Email,
                Email = user.Email

            };

           var dbActionResult = await _userManager.CreateAsync(identityUser, user.Password);

           return dbActionResult.Succeeded ? true : false;
        }
    }
}
