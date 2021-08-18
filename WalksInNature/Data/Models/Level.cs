using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Level;

namespace WalksInNature.Data.Models
{
    public class Level
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
        public ICollection<Walk> Walks { get; set; } = new HashSet<Walk>();
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
