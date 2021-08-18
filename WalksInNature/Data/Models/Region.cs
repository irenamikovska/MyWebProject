using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Region;

namespace WalksInNature.Data.Models
{
    public class Region
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public ICollection<Walk> Walks { get; set; } = new HashSet<Walk>();
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
