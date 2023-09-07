using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Dtos;
using StudentCRUD.Dtos.Auth;
using StudentCRUD.Repository;
using StudentCRUD.Services.AuthService;
using StudentCRUD.Services.Email;

namespace StudentCRUD.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AuthController(IAuthRepository authRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]UserRegisterRequest user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest();
                }
                if(await _userRepository.IsEmailExistAsync(user.Email))
                {
                    return BadRequest("Email Already Exist");
                }

                var isRegistered = await _authRepository.RegisterAsync(user);

                if (isRegistered)
                {
                    return Ok();
                }

                return StatusCode(500, "Internal Server issue!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]UserLoginRequest user)
        {

            try
            {
                if (user is null)
                {
                    return BadRequest();
                }

                if (!await _userRepository.IsUserExistAsync(user.Email, user.Password))
                {
                    return BadRequest("Wrong Credential");
                }

                var loginResponse = await _authRepository.LoginAsync(user);

                if(loginResponse)
                {
                    var tokenFactory = new JwtTokenFactory();

                    var tokenResponse = tokenFactory.CreateJWT(await _userRepository.GetUserForTokenAsync(user.Email));

                    return Ok(tokenResponse);
                   
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm]string email)
        {
           if(string.IsNullOrEmpty(email))
           {
                return BadRequest();
           }
            var res = await _authRepository.ForgotPassword(email);

            if (res)
            {
                return Ok("email send");
            }
            return BadRequest();
        }
        [HttpGet("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string userId , [FromQuery] string token)
        {
            return Ok(token);
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest resetPassword)
        {
            return Ok();
        }

    }
}





