using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.DataAccess;
using StudentCRUD.Dtos;
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

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is not null)
            {
                var token = "abc";
                string appDomain = $@"https://localhost:7257/api/";
                string confirmLink = $"{appDomain}resetPassword?userId={user.Id}&token={token}";

                var placeHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("userName" , user.UserName),
                    new KeyValuePair<string, string>("link" ,confirmLink)
                };
                var emailOption = new UserEmailOption
                {
                    ToEmail = user.Email,
                    Body = "",
                    Subject = "Reset Password",
                    PlaceHolder = placeHolders,
                    TemplateName = "EmailTemplate"
                    

                };

                await _emailService.SendEmail(emailOption);

                return true;
            }

            return false;
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
    }
}
