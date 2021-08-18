using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Guide;

namespace WalksInNature.Data.Models
{
    public class Guide
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Event> Events { get; init; } = new HashSet<Event>();
    }
}
