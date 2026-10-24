using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels;

public class InstructorViewModel
{
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstMidName { get; set; }

    public DateTime HireDate { get; set; }
    public string OfficeLocation { get; set; }
    public List<int> CourseIDs { get; set; }
}
