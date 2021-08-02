using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalksInNature.Data.Models
{
    public class WalkUser
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int WalkId { get; set; }

        public Walk Walk { get; set; }
    }
}
