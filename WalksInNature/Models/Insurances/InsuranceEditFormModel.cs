using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Insurance;

namespace WalksInNature.Models.Insurances
{
    public class InsuranceEditFormModel
    {
       
        [Required]
        [StringLength(BeneficiaryMaxLength, MinimumLength = BeneficiaryMinLength, ErrorMessage = "The {0} can be between {2} and {1} characters")]
        [Display(Name = "Full Name and EGN of Beneficiary")]
        public string Beneficiary { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}
