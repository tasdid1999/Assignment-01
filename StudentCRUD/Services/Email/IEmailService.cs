namespace StudentCRUD.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(UserEmailOption userEmail);
        
    }
}
