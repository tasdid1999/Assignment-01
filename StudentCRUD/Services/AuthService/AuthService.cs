using Microsoft.AspNetCore.Identity;
using StudentCRUD.Dtos.Auth;
using StudentCRUD.Dtos.User;
using StudentCRUD.Repository;
using StudentCRUD.Services.Email;

namespace StudentCRUD.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public AuthService(UserManager<IdentityUser> userManger, IEmailService emailService, IUserRepository userRepository)
        {
            _userManager = userManger;
            _emailService = emailService;
            _userRepository = userRepository;
        }
        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

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

        public LoginResponse GetJwtToken(UserForToken user)
        {
            var tokenFactory = new JwtTokenFactory();
            var loginresponse = tokenFactory.CreateJWT(user);

            return loginresponse;

        }
    }
}
