using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Insurances;
using WalksInNature.Services.Insurances;

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
        public IActionResult Details(string insuranceId)
        {
           
            var insuranceDetails = this.insuranceService.GetDetails(insuranceId);                

            return this.View(insuranceDetails);
        }



        [Authorize]
        public IActionResult Add() => View();


        [HttpPost]
        [Authorize]
        public IActionResult Add(InsuranceFormModel input)
        {
                       
            //var startIns = input.StartDate;
            //var endIns = DateTime.ParseExact(input.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var duration = (input.EndDate - input.StartDate).Days;
            
            
            if(input.StartDate < DateTime.UtcNow)
            {
                this.ModelState.AddModelError(nameof(input.StartDate), "Start date have to after current date!");
            }

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
                input.StartDate,                
                input.EndDate,                
                input.NumberOfPeople,
                input.Limit,                
                input.Beneficiary,
                userId
                );
            
            return RedirectToAction(nameof(MyInsurances));
           
        }

       

    }
}
