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

        public IEnumerable<Walk> Walks { get; init; } = new List<Walk>();
        
        public IEnumerable<Insurance> Insurances { get; init; } = new List<Insurance>();

        public ICollection<WalkUser> Likes { get; init; } = new List<WalkUser>();

        public virtual ICollection<EventUser> Events { get; set; } = new HashSet<EventUser>();
    }
}
