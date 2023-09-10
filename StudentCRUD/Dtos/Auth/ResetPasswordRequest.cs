using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.Dtos.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and confirm password should be same")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }

        [Required]
        public string token { get; set; }




    }
}
