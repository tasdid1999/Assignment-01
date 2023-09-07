namespace StudentCRUD.Dtos.Auth
{
    public class LoginResponse
    {
        public string? Token { get; set; }

        public DateTime Expire { get; set; }
    }
}
