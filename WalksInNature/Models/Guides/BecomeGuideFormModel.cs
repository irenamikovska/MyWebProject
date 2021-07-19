using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Guide;

namespace WalksInNature.Models.Guides
{
    public class BecomeGuideFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PhoneMaxLength, MinimumLength = PhoneMinLength)]
        [Display(Name = "Phone Number")]
        //[RegularExpression()]
        public string PhoneNumber { get; set; }
    }
}
