using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants;

namespace WalksInNature.Models.Walks
{
    public class AddWalkFormModel
    {
        [Required]       
        [StringLength(WalkNameMaxLength, MinimumLength = WalkNameMinLength, ErrorMessage="The {0} should be between {2} and {1} characters")]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [StringLength(WalkStartPointMaxLength, MinimumLength = WalkStartPointMinLength, ErrorMessage = "The {0} should be between {2} and {1} characters")]
        [Display(Name = "Start point")]
        public string StartPoint { get; init; }

        [Display(Name = "Region")]
        public int RegionId { get; init; }

        [Display(Name = "Level")]
        public int LevelId { get; init; }     
        
        [Required]
        [StringLength(int.MaxValue, MinimumLength = WalkDescriptionMinLength, ErrorMessage = "The {0} should be at least {2} characters")]
        public string Description { get; init; }
        public IEnumerable<WalkRegionViewModel> Regions { get; set; }
        public IEnumerable<WalkLevelViewModel> Levels { get; set; }
    }
}