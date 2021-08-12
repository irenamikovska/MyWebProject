using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Services.Insurances.Models;
using WalksInNature.Infrastructure;

namespace WalksInNature.Services.Insurances
{
    public class InsuranceService : IInsuranceService
    {
        private readonly WalksDbContext data;
        private readonly IMapper mapper;
        public InsuranceService(WalksDbContext data, IMapper mapper) 
        {
            this.data = data;
            this.mapper = mapper;
        } 
        
        public IEnumerable<InsuranceServiceModel> InsurancesByUser(string userId)
        {
            var myInsurances = this.data
                .Insurances
                .Where(x => x.UserId == userId)
                .Select(x => new InsuranceServiceModel
                {
                    Id = x.Id,
                    UserId = userId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    NumberOfPeople = x.NumberOfPeople,
                    TotalPrice = x.TotalPrice
                })
                .OrderBy(d => d.StartDate)
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
                   StartDate = x.StartDate,
                   EndDate = x.EndDate,
                   Duration = x.Duration,
                   Limit = x.Limit,
                   NumberOfPeople = x.NumberOfPeople,
                   PricePerPerson = x.PricePerPerson,
                   TotalPrice = x.TotalPrice,
                   RefNumber = x.Id.Substring(0, 8),
                   Beneficiary = x.Beneficiary,
                   UserId = x.UserId
               })
               .FirstOrDefault();
       

        public string Book(string startDate, string endDate, int numberOfPeople, int limit, string beneficiary, string userId)
        {
           
            var start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var duration = (end - start).Days;
            var pricePerPerson = CalulatePrice(duration, limit);
            var totalPrice = numberOfPeople * pricePerPerson;                        

            var insuranceToBook = new Insurance
            {
                StartDate = start,
                EndDate = end,
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

        public bool Edit(string id, string startDate, string endDate, int numberOfPeople, int limit, string beneficiary)
        {
            var insuranceData = this.data.Insurances.Find(id);

            if (insuranceData == null)
            {
                return false;
            }

            var start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var duration = (end - start).Days;
            var pricePerPerson = CalulatePrice(duration, limit);
            var totalPrice = numberOfPeople * pricePerPerson;

            insuranceData.StartDate = start;
            insuranceData.EndDate = end;
            insuranceData.Duration = duration;
            insuranceData.NumberOfPeople = numberOfPeople;
            insuranceData.Limit = limit;
            insuranceData.PricePerPerson = pricePerPerson;
            insuranceData.TotalPrice = totalPrice;
            insuranceData.Beneficiary = beneficiary;
           
            this.data.SaveChanges();

            return true;
        }

        public void Delete(string id)
        {
            var insuranceToDelete = this.data.Insurances.Find(id);

            if (insuranceToDelete != null)
            {
                this.data.Insurances.Remove(insuranceToDelete);
                this.data.SaveChanges();
            }
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
