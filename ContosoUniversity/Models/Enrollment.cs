using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContosoUniversity.Models;

public enum Grade
{
    A, B, C, D, F
}

public class Enrollment
{
    public int EnrollmentID { get; set; }
    public int CourseID { get; set; }
    public int StudentID { get; set; }
    [DisplayFormat(NullDisplayText = "No grade")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Grade? Grade { get; set; }

    [JsonIgnore]
    public Course Course { get; set; }
    [JsonIgnore]
    public Student Student { get; set; }

    public string CourseTitle
    {
        get => Course == null ? string.Empty : Course.Title;
    }
}