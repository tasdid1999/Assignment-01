using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Dtos;
using StudentCRUD.Repository;
using StudentCRUD.Services.AuthService;

namespace StudentCRUD.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        
        public AuthController(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
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
    }
}





