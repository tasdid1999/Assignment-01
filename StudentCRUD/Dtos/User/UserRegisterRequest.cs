using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.Dtos
{
    public class UserRegisterRequest
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
