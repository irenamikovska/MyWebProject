using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Guide;

namespace WalksInNature.Models.Guides
{
    public class BecomeGuideFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The {0} should be between {2} and {1} characters")]       
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression("\\+[0-9]{2,}-[0-9]{2,}-[0-9]{3,}-[0-9]{3,}", ErrorMessage = "The number should be in format +XX-XX-XXX-XXX between 10 and 30 symbols")]       
        public string PhoneNumber { get; set; }
    }
}
