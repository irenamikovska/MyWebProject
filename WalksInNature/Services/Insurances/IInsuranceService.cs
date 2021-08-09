using System;
using System.Collections.Generic;
using WalksInNature.Services.Insurances.Models;

namespace WalksInNature.Services.Insurances
{
    public interface IInsuranceService
    {
        string Book(string startDate, string endDate, int numberOfPeople, int limit, string beneficiary, string userId);

        InsuranceDetailsServiceModel GetDetails(string insuranceId);
        IEnumerable<InsuranceServiceModel> InsurancesByUser(string userId);
        
    }
}
