﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FlagsX0.Web.Data.Entities
{
    public class FlagEntity
    {
        [Key] public int Id { get; set; }
        public required string Name { get; set; }
        public IdentityUser User { get; set; }
        public required virtual string UserId { get; set; }
        public required bool Value { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedTimeUtc { get; set; }
    }
}
