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
        private readonly IAuthService _authService;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService,
                              IUserRepository userRepository,
                              IEmailService emailService,
                              IAuthRepository authRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
            _emailService = emailService;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
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
        


        [HttpPost("login")]
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

                var isLogInSucces = await _authRepository.LoginAsync(user);

                if(isLogInSucces)
                {
                    var  loginResponse = _authService.GetJwtToken(await _userRepository.GetUserForTokenAsync(user.Email));

                    return Ok(loginResponse);
                   
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
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest();
                }
                var res = await _authService.ForgotPassword(email);

                return res ? Ok() : BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("resetPassword")]
        public  IActionResult ResetPassword([FromQuery] string userId , [FromQuery] string token)
        {
            return Ok(new {Token = token});
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordRequest resetPassword)
        {
            try
            {
                var result = await _authRepository.ResetPassword(resetPassword);

                return result ? Ok("password update succesful!") : BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}





