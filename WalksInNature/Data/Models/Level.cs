using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants;

namespace WalksInNature.Data.Models
{
    public class Level
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(LevelNameMaxLength)]
        public string Name { get; set; }
        public IEnumerable<Walk> Walks { get; set; }
    }
}
