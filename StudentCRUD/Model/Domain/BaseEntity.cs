namespace StudentCRUD.Model.Domain
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; }
    }
}
