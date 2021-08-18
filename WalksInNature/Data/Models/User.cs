using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.User;

namespace WalksInNature.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<Walk> Walks { get; init; } = new HashSet<Walk>();
        
        public ICollection<Insurance> Insurances { get; init; } = new HashSet<Insurance>();

        public ICollection<WalkUser> Likes { get; init; } = new HashSet<WalkUser>();

        public ICollection<EventUser> Events { get; set; } = new HashSet<EventUser>();
    }
}
