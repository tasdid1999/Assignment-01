using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentCRUD.Dtos.Auth;
using StudentCRUD.Dtos.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentCRUD.Services.AuthService
{
    public class JwtTokenFactory
    {
        public LoginResponse CreateJWT(UserForToken user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("secretKeyveryimportant123456789");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Name, user.UserName)
            });

            var credential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credential
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var accessToken =  jwtTokenHandler.WriteToken(token);

            return new LoginResponse()
            {
                Token = accessToken,
                Expire = (DateTime)tokenDescriptor.Expires,
            };
        }
    }
}
