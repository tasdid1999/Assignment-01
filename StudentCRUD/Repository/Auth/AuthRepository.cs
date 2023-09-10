using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.DataAccess;
using StudentCRUD.Dtos;
using StudentCRUD.Dtos.Auth;
using StudentCRUD.Services.Email;

namespace StudentCRUD.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;

        public AuthRepository(UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<IdentityRole> roleManager,
                             ApplicationDbContext dbContext,
                             IEmailService emailService
                             )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _emailService = emailService;
            

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
                Email = user.Email,
            };
            
            
           var dbActionResult = await _userManager.CreateAsync(identityUser, user.Password);
           
           if(dbActionResult.Succeeded)
           {
                var newUser = await _userManager.FindByEmailAsync(identityUser.Email);

                if(!await _roleManager.RoleExistsAsync(user.Role))
                {
                   await _roleManager.CreateAsync(new IdentityRole(user.Role));
                }
                await _userManager.AddToRoleAsync(newUser, user.Role);
                
                
           }

           return dbActionResult.Succeeded ? true : false;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest resetPassword)
        {
            var user = await _userManager.FindByIdAsync(resetPassword.UserId);
            var result = await _userManager.ResetPasswordAsync(user, resetPassword.token, resetPassword.ConfirmPassword);

            if(result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
