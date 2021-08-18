using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalksInNature.Services.Events.Models;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;
using static WalksInNature.Data.Models.DataConstants.Event;

namespace WalksInNature.Models.Events
{
    public class EventFormModel : IEventModel
    {
        
        [Required]       
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage="The {0} should be between {2} and {1} characters")]
        public string Name { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [StringLength(StartPointMaxLength, MinimumLength = StartPointMinLength, ErrorMessage = "The {0} should be between {2} and {1} characters")]
        [Display(Name = "Start point")]
        public string StartPoint { get; init; }

        [Range(1, 100)]
        [Display(Name = "Region")]
        public int RegionId { get; init; }

        [Range(1, 10)]
        [Display(Name = "Level")]
        public int LevelId { get; init; }   
        
        [Required]
        [Display(Name = "Date of event")]
        [DataType(DataType.Date)]
        public DateTime Date { get; init; }

        [Required]
        [Display(Name = "Start Hour")]       
        [RegularExpression("[0-9]{2}:[0-9]{2}", ErrorMessage = "The hour should be in format HH:mm")]
        public string StartHour { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = DescriptionMinLength, ErrorMessage = "The {0} should be at least {2} characters")]
        public string Description { get; init; }

        public int GuideId { get; set; }
        
        public IEnumerable<RegionServiceModel> Regions { get; set; }
        public IEnumerable<LevelServiceModel> Levels { get; set; }

    }
}