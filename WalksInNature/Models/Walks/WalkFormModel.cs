﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalksInNature.Services.Levels;
using WalksInNature.Services.Regions;
using WalksInNature.Services.Walks.Models;
using static WalksInNature.Data.Models.DataConstants.Walk;

namespace WalksInNature.Models.Walks
{
    public class WalkFormModel : IWalkModel
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
        [Display(Name = "Start Point")]
        public string StartPoint { get; init; }

        [Display(Name = "Region")]
        public int RegionId { get; init; }

        [Display(Name = "Level")]
        public int LevelId { get; init; }     
        
        [Required]
        [StringLength(int.MaxValue, MinimumLength = DescriptionMinLength, ErrorMessage = "The {0} should be at least {2} characters")]
        public string Description { get; init; }

        public string UserId { get; set; }
        public IEnumerable<RegionServiceModel> Regions { get; set; }
        public IEnumerable<LevelServiceModel> Levels { get; set; }
    }
}