
namespace WalksInNature.Services.Insurances.Models
{
    public class InsuranceDetailsServiceModel
    {
        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Duration { get; set; }
        public int Limit { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal PricePerPerson { get; set; }
        public decimal TotalPrice { get; set; }
        public string Beneficiary { get; set; }
        public string RefNumber { get; set; }

    }
}
