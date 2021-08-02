using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Services.Insurances.Models;

namespace WalksInNature.Services.Insurances
{
    public class InsuranceService : IInsuranceService
    {
        private readonly WalksDbContext data;
        public InsuranceService(WalksDbContext data) => this.data = data;

        public IEnumerable<InsuranceServiceModel> InsurancesByUser(string userId)
        {
            var myInsurances = this.data
                .Insurances
                .Where(x => x.UserId == userId)
                .Select(x => new InsuranceServiceModel
                {
                    Id = x.Id,
                    UserId = userId,
                    StartDate = x.StartDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    NumberOfPeople = x.NumberOfPeople,
                    TotalPrice = x.TotalPrice
                })
                .OrderByDescending(d => d.StartDate)
                .ToList();

            return myInsurances;
            
        }              

        public InsuranceDetailsServiceModel GetDetails(string insuranceId)
        
            => this.data
               .Insurances
               .Where(x => x.Id == insuranceId)
               .Select(x => new InsuranceDetailsServiceModel
               {
                   Id = x.Id,
                   StartDate = x.StartDate.ToString(),
                   EndDate = x.EndDate.ToString(),
                   Duration = x.Duration,
                   Limit = x.Limit,
                   NumberOfPeople = x.NumberOfPeople,
                   PricePerPerson = x.PricePerPerson,
                   TotalPrice = x.TotalPrice,
                   RefNumber = x.Id.Substring(0, 8),
                   Beneficiary = x.Beneficiary
               })
               .FirstOrDefault();
       

        public string Book(DateTime startDate, DateTime endDate, int numberOfPeople, int limit, string beneficiary, string userId)
        {         

            //var start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //var end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var duration = (endDate - startDate).Days;
            var pricePerPerson = CalulatePrice(duration, limit);
            var totalPrice = numberOfPeople * pricePerPerson;                        

            var insuranceToBook = new Insurance
            {
                StartDate = startDate,
                EndDate = endDate,
                Duration = duration,
                NumberOfPeople = numberOfPeople,
                Limit = limit,
                PricePerPerson = pricePerPerson,
                TotalPrice = totalPrice,
                Beneficiary = beneficiary,
                UserId = userId
            };

            this.data.Insurances.Add(insuranceToBook);
            this.data.SaveChanges();

            return insuranceToBook.Id;
        }

        private decimal CalulatePrice(int duration, int limit)
        {
            decimal result = 0;

            if (duration > 0 && duration <= 3)
            {
                result = limit * 0.0015m;
            }

            else if (duration > 3 && duration <= 7)
            {
                result = limit * 0.0025m;
            }

            else if (duration > 7 && duration <= 10)
            {
                result = limit * 0.0030m;
            }

            else if (duration > 10 && duration <= 15)
            {
                result = limit * 0.0035m;
            }

            else if (duration > 15 && duration <= 30)
            {
                result = limit * 0.0055m;
            }

            else if (duration > 30 && duration <= 360)
            {
                result = limit * 0.0150m;
            }

            return result;
        }

       
    }
}
