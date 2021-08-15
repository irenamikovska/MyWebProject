using System.Collections.Generic;

namespace WalksInNature.Services.Insurances.Models
{
    public class InsuranceQueryServiceModel
    {
        public int CurrentPage { get; init; }
        public int InsurancesPerPage { get; init; }
        public int TotalInsurances { get; init; }
        public IEnumerable<InsuranceServiceModel> Insurances { get; init; }

    }
}
