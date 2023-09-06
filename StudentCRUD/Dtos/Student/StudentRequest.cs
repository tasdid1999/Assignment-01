using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.Dtos
{
    public class StudentRequest
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }
    }
}
