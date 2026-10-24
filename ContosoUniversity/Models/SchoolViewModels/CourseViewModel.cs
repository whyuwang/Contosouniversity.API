using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels;

public class CourseViewModel
{
    [Required]
    public int CourseID { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string Title { get; set; }

    [Range(0, 5)]
    public int Credits { get; set; }
    public int DepartmentID { get; set; }
    public string DepartmentName { get; set; }
}
