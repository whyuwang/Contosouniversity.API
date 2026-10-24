using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContosoUniversity.Models;

public class OfficeAssignment
{
    [Key]
    public int InstructorID { get; set; }
    [StringLength(50)]
    [Display(Name = "Office Location")]
    public string Location { get; set; }

    [JsonIgnore]
    public Instructor Instructor { get; set; }
}
