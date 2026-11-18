using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public abstract class Person : EntityBase
{
    [MaxLength(50)]
    public required string LastName { get; set; }
    [MaxLength(50)]
    public required string FirstName { get; set; }
}
