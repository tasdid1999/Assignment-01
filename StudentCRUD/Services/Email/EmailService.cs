using Microsoft.Extensions.Options;
using StudentCRUD.Model.Helper;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace StudentCRUD.Services.Email
{
    public class EmailService : IEmailService
    {
        private const string EmailPath = @"Template/{0}.html";
        private readonly SmtpConfig _smtpConfig;
        public EmailService(IOptions<SmtpConfig> configuration)
        {
            _smtpConfig = configuration.Value;
        }
        public async Task SendEmail(UserEmailOption userEmail)
        {
            var mail = new MailMessage()
            {

                Subject = userEmail.Subject,
                Body = UpdatePlaceHolder(GetEmailBody(userEmail.TemplateName),userEmail.PlaceHolder),
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML,

            };
            
           
            mail.To.Add(userEmail.ToEmail);

            var networkCredential = new NetworkCredential(_smtpConfig.UserName,_smtpConfig.Password);

            var smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port ,
                UseDefaultCredentials = _smtpConfig.UserDefaultCredentails ,
                EnableSsl = _smtpConfig.EnableSSL,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
              
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(EmailPath,templateName));

            return body;
        }
        private string UpdatePlaceHolder(string text , List<KeyValuePair<string , string>> placeHolder)
        {
            if(!string.IsNullOrEmpty(text) && placeHolder is not null)
            {
                foreach(var item in placeHolder)
                {
                    if(text.Contains(item.Key))
                    {
                        text = text.Replace(item.Key, item.Value);
                    }
                       
                }
            }

            return text;
        }
        
    }
}
