using System.ComponentModel.DataAnnotations;

namespace WalksInNature.Data.Models
{
    public class WalkUser
    {
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int WalkId { get; set; }

        public Walk Walk { get; set; }
    }
}
