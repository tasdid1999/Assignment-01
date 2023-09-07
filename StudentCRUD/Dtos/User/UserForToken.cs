using Microsoft.AspNetCore.Identity;

namespace StudentCRUD.Dtos.User
{
    public class UserForToken
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public UserForToken(IdentityUser user , string role)
        {
            Id = user.Id;
            UserName = user.UserName;
            Role = role;

        }


    }
}
