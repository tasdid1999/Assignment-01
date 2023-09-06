using System.ComponentModel.DataAnnotations;

namespace StudentCRUD.Model.Domain
{
    public class Student : BaseEntity
    {
       
        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }
    }
}
