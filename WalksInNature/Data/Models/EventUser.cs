using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalksInNature.Data.Models
{
    public class EventUser
    {
        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}
