using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public class CreateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Course { get; set; }
    }
}
