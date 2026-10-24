using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models.SchoolViewModels;

public class StudentViewModel
{
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string FirstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
