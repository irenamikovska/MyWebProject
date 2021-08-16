using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalksInNature.Data.Models;

using static WalksInNature.Test.Data.DataConstants;

namespace WalksInNature.Test.Data
{
    public static class Insurances
    {
        public static IEnumerable<Insurance> GetInsurances
            => Enumerable.Range(1, 10).Select(x => new Insurance
            {
                StartDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 1, 3),
                NumberOfPeople = 2,
                Limit = 2000,
                Beneficiary = "FullNameAndEGNOfBeneficiary"
            });

        public static Insurance GetInsurance(string id = TestIdGuid, bool isPaid = false)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username,
            };

            return new Insurance
            {
                Id = id,
                IsPaid = isPaid,
                StartDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 1, 3),
                NumberOfPeople = 2,
                Limit = 2000,
                Beneficiary = "FullNameAndEGNOfBeneficiary",
                UserId = user.Id
            };
        }
    }
}


                   