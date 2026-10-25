using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels;

public class DepartmentViewModel
{
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; }

    [DataType(DataType.Currency)]
    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int? InstructorID { get; set; }
}
