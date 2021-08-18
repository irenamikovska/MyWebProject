using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Services.Insurances.Models;
using WalksInNature.Models.Insurances;

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
        public InsuranceQueryServiceModel All(
               string userId = null,
               string searchTerm = null,
               InsuranceSorting sorting = InsuranceSorting.DateCreated,
               int currentPage = 1,
               int insurancesPerPage = int.MaxValue)
        {

            var insurancesQuery = this.data.Insurances.AsQueryable();
                
            if (!string.IsNullOrWhiteSpace(userId))
            {
                insurancesQuery = insurancesQuery.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                insurancesQuery = insurancesQuery.Where(x =>
                    x.Beneficiary.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Id.ToLower().Contains(searchTerm.ToLower()));
            }

            insurancesQuery = sorting switch
            {
                InsuranceSorting.StartDate => insurancesQuery.OrderBy(x => x.StartDate),
                InsuranceSorting.EndDate => insurancesQuery.OrderBy(x => x.EndDate),
                InsuranceSorting.TotalPrice => insurancesQuery.OrderBy(x => x.TotalPrice),
                InsuranceSorting.DateCreated or _ => insurancesQuery.OrderByDescending(x => x.Id)
            };

            var totalInsurances = insurancesQuery.Count();

            var insurances = GetInsurances(insurancesQuery
                .Skip((currentPage - 1) * insurancesPerPage)
                .Take(insurancesPerPage));

            return new InsuranceQueryServiceModel
            {
                TotalInsurances = totalInsurances,
                CurrentPage = currentPage,
                InsurancesPerPage = insurancesPerPage,
                Insurances = insurances
            };

        }

        public IEnumerable<string> AllUserId()
           => this.data.Insurances
               .Select(x => x.UserId)
               .Distinct()
               .OrderBy(d => d)
               .ToList();

        
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
                    TotalPrice = x.TotalPrice,
                    IsPaid = x.IsPaid
                })
                .OrderBy(d => d.StartDate)
                .ToList();

            return myInsurances;            
        }      
               
        public InsuranceDetailsServiceModel GetDetails(string insuranceId)
        
            => this.data
               .Insurances
               .Where(x => x.Id == insuranceId)
               .ProjectTo<InsuranceDetailsServiceModel>(this.mapper.ConfigurationProvider)              
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
                UserId = userId,
                IsPaid = false
            };

            this.data.Insurances.Add(insuranceToBook);
            this.data.SaveChanges();

            return insuranceToBook.Id;
        }

        public bool Edit(string insuranceId, string beneficiary)
        {
            var insuranceData = this.data.Insurances.Find(insuranceId);

            if (insuranceData == null)
            {
                return false;
            }                      
                        
            insuranceData.Beneficiary = beneficiary;

            this.data.SaveChanges();

            return true;
        }
       
        public void ChangeStatus(string insuranceId)
        {
            var insurance = this.data.Insurances.Find(insuranceId);

            insurance.IsPaid = !insurance.IsPaid;

            this.data.SaveChanges();
        }

        public void DeleteByUser(string id, string userId)
        {
            var insuranceToDelete = this.data.Insurances.Find(id);

            if (insuranceToDelete != null || insuranceToDelete.UserId == userId)
            {
                this.data.Insurances.Remove(insuranceToDelete);
                this.data.SaveChanges();
            }
        }       

        public void DeleteByAdmin(string id)
        {
            var insuranceToDelete = this.data.Insurances.Find(id);

            if (insuranceToDelete != null)
            {
                this.data.Insurances.Remove(insuranceToDelete);
                this.data.SaveChanges();
            }
        }
        private IEnumerable<InsuranceServiceModel> GetInsurances(IQueryable<Insurance> insuranceQuery)
            => insuranceQuery
                .ProjectTo<InsuranceServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

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
