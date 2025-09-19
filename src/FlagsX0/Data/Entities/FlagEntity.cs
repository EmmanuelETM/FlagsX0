using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlagsX0.Data.Entities;

public class FlagEntity
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public IdentityUser User { get; set; }
    public virtual required string UserId { get; set; }
    public required bool Value { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedTimeUtc { get; set; }
}