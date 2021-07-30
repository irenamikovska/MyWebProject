using System;
using System.ComponentModel.DataAnnotations;
using static WalksInNature.Data.Models.DataConstants.Insurance;

namespace WalksInNature.Data.Models
{
    public class Insurance
    {
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int NumberOfPeople { get; set; }      
        public int Limit { get; set; }
        public decimal PricePerPerson { get; set; }
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(BeneficiaryMaxLength)]
        public string Beneficiary { get; set; }        

        [Required]
        public string UserId { get; set; }
        public User User { get; init; }
    }
}
