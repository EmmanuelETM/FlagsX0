using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlagsX0.Data.Entities;

public sealed class FlagEntity
{
    [Key] public int Id { get; set; }
    public required string Name { get; set; }
    public required IdentityUser User { get; set; }
    public required string UserId { get; set; }
    public required bool Value { get; set; }
}