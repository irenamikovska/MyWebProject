using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalksInNature.Services.Insurances.Models;

namespace WalksInNature.Models.Insurances
{
    public class AllInsuranceQueryModel
    {
        // for searching       

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }
        public IEnumerable<InsuranceServiceModel> Insurances { get; set; }

        // for filtering by userId
        public string UserId { get; init; }
        public IEnumerable<string> UserIds { get; set; }

        // for sorting
        public InsuranceSorting Sorting { get; init; }

        // for paging

        public const int InsurancesPerPage = 10;
        public int CurrentPage { get; init; } = 1;
        public int TotalInsurance { get; set; }
    }
}
