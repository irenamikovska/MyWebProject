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
        public IEnumerable<Walk> Walks { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
