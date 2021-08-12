using System;

namespace WalksInNature.Services.Insurances.Models
{
    public class InsuranceServiceModel
    {
        public string Id { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFormated => this.StartDate.Date.ToString("dd.MM.yyyy");
        public DateTime EndDate { get; set; }
        public string EndDateFormated => this.EndDate.Date.ToString("dd.MM.yyyy");
        public int NumberOfPeople { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }

    }
}

