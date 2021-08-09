using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Insurances;
using WalksInNature.Services.Insurances;

using static WalksInNature.WebConstants;

namespace WalksInNature.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly IInsuranceService insuranceService;
        
        public InsurancesController(IInsuranceService insuranceService)
        {
            this.insuranceService = insuranceService;            
        }

        [Authorize]
        public IActionResult MyInsurances()
        {
            var userId = this.User.GetId();

            var myInsurances = this.insuranceService.InsurancesByUser(userId);

            return View(myInsurances);
        }

        [Authorize]
        public IActionResult Details(string id)
        {
           
            var insuranceDetails = this.insuranceService.GetDetails(id);                

            return this.View(insuranceDetails);
        }

        [Authorize]
        public IActionResult Add() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Add(InsuranceFormModel input)
        {
               
            if(input.StartDate < DateTime.UtcNow)
            {
                this.ModelState.AddModelError(nameof(input.StartDate), "Start date have to after current date!");
            }

            var duration = (input.EndDate - input.StartDate).Days;

            if (duration < 0) 
            {
                this.ModelState.AddModelError(nameof(input.EndDate), "End date have to be after start date!");
            }

            if (duration > 360)
            {
                this.ModelState.AddModelError(nameof(input.EndDate), "End date have to be not more 360 days!");
            }

            if (input.Limit != 2000 || input.Limit != 5000 || input.Limit != 10000)
            {
                this.ModelState.AddModelError(nameof(input.Limit), "Limit can be 2000, 5000 or 10000!");
            }
            
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var userId = this.User.GetId();

            this.insuranceService.Book(
                input.StartDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),                
                input.EndDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),                
                input.NumberOfPeople,
                input.Limit,                
                input.Beneficiary,
                userId
                );

            TempData[GlobalMessageKey] = "You insurance was booked!";

            return RedirectToAction(nameof(MyInsurances));           
        }       

    }
}
