using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels;

public class EnrollmentDateGroup
{
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}
