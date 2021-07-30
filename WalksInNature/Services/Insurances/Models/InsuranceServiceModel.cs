namespace WalksInNature.Services.Insurances.Models
{
    public class InsuranceServiceModel
    {
        public string Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }

    }
}

