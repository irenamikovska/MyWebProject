using System;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Insurance;


namespace WalksInNature.Models.Insurances
{
    public class InsuranceFormModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]       
        public DateTime StartDate { get; init; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]        
        public DateTime EndDate { get; init; }
        
        [Range(PersonsMinValue, PersonsMaxValue)]
        [Display(Name = "Number Of People")]
        public int NumberOfPeople { get; set; }

        [Range(LimitMinValue, LimitMaxValue)]
        [Display(Name = "Limit/Coverage")]
        public int Limit { get; set; }        

        [Required]
        [StringLength(BeneficiaryMaxLength, MinimumLength = BeneficiaryMinLength, ErrorMessage = "The {0} can be between {2} and {1} characters")]
        [Display(Name = "Full Name and EGN of Beneficiary")]
        public string Beneficiary { get; set; }

        [Required]
        public string UserId { get; set; }
        
    }
}
