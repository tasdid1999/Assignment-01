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
        public string ConfirmPassword { get; set; }

        [Required]
        public string token { get; set; }




    }
}
