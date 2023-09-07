namespace StudentCRUD.Services.Email
{
    public class UserEmailOption
    {
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string TemplateName { get; set; }

        public List<KeyValuePair<string, string>> PlaceHolder { get; set; }
    }
}
