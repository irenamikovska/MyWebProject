using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Event;

namespace WalksInNature.Data.Models
{
    public class Event
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
      
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(StartPointMaxLength)]
        public string StartPoint { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int LevelId { get; set; }
        public Level Level { get; set; }

        public DateTime Date { get; set; }
        public DateTime StartingHour { get; set; }

        [Required]       
        public string Description { get; set; }

        public int GuideId { get; set; }
        public Guide Guide { get; init; }  
        
        public virtual ICollection<EventUser> Users { get; set; } = new HashSet<EventUser>();

    }
}
