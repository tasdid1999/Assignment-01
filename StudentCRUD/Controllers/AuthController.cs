using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentCRUD.Dtos;
using StudentCRUD.Repository;

namespace StudentCRUD.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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
                if(await _authRepository.IsEmailExistAsync(user.Email))
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

                if (!await _authRepository.IsUserExistAsync(user.Email, user.Password))
                {
                    return BadRequest("Wrong Credential");
                }

                var loginResponse = await _authRepository.LoginAsync(user);

                return loginResponse ? Ok() : StatusCode(500, "Internal Server Error");
            }


            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}





