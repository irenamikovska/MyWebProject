namespace WalksInNature.Services.Insurances.Models
{
    public class InsuranceDetailsServiceModel : InsuranceServiceModel
    {
        public int Duration { get; set; }
        public int Limit { get; set; }       
        public decimal PricePerPerson { get; set; }       
        public string Beneficiary { get; set; }
        public string RefNumber { get; set; }
        
    }
}


