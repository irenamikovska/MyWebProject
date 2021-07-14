using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants;

namespace WalksInNature.Data.Models
{
    public class Walk
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(WalkNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string StartPoint { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; init; }
        public int LevelId { get; set; }
        public Level Level { get; init; }

        [Required]        
        public string Description { get; set; }

    }
}
