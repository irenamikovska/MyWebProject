using System.Collections.Generic;
using WalksInNature.Models.Insurances;
using WalksInNature.Services.Insurances.Models;

namespace WalksInNature.Services.Insurances
{
    public interface IInsuranceService
    {
       InsuranceQueryServiceModel All(string userId = null,
               string searchTerm = null,
               InsuranceSorting sorting = InsuranceSorting.DateCreated,
               int currentPage = 1,
               int insurancesPerPage = int.MaxValue);

        IEnumerable<string> AllUserId();
        string Book(string startDate, string endDate, int numberOfPeople, int limit, string beneficiary, string userId);
        InsuranceDetailsServiceModel GetDetails(string insuranceId);
        IEnumerable<InsuranceServiceModel> InsurancesByUser(string userId);
        bool Edit(string insuranceId, string beneficiary);        
        void ChangeStatus(string insuranceId);
        void DeleteByUser(string id, string userId);
        void DeleteByAdmin(string id);
    }
}
