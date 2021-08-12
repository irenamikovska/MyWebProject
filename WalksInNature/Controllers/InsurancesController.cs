using AutoMapper;
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
        private readonly IMapper mapper;
        public InsurancesController(IInsuranceService insuranceService, IMapper mapper)
        {
            this.insuranceService = insuranceService;
            this.mapper = mapper;
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
               
            if(input.StartDate.Date < DateTime.UtcNow)
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
            /*
            if (!ModelState.IsValid)
            {
                return View(input);
            }*/

            var userId = this.User.GetId();

            this.insuranceService.Book(
                input.StartDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),                
                input.EndDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),                
                input.NumberOfPeople,
                input.Limit,                
                input.Beneficiary,
                userId
                );

            TempData[GlobalMessageKey] = "Your insurance was booked!";

            return RedirectToAction(nameof(MyInsurances));           
        }
       
        [Authorize]
        public IActionResult Edit(string id)
        {
            var userId = this.User.GetId();

            var insurance = this.insuranceService.GetDetails(id);
           
            if (insurance.UserId != userId)
            {
                return Unauthorized();
            }
                        
            var insuranceForm = this.mapper.Map<InsuranceFormModel>(insurance);

            return View(insuranceForm);

        }
        

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string id, InsuranceFormModel insurance)
        {
            
            if (insurance.StartDate.Date < DateTime.UtcNow)
            {
                this.ModelState.AddModelError(nameof(insurance.StartDate), "Start date have to after current date!");
            }

            var duration = (insurance.EndDate - insurance.StartDate).Days;

            if (duration < 0)
            {
                this.ModelState.AddModelError(nameof(insurance.EndDate), "End date have to be after start date!");
            }

            if (duration > 360)
            {
                this.ModelState.AddModelError(nameof(insurance.EndDate), "End date have to be not more 360 days!");
            }

            if (insurance.Limit != 2000 || insurance.Limit != 5000 || insurance.Limit != 10000)
            {
                this.ModelState.AddModelError(nameof(insurance.Limit), "Limit can be 2000, 5000 or 10000!");
            }
                /*      
            if (!ModelState.IsValid)
            {
                return View(insurance);
            }*/

            var insuranceToEdit = this.insuranceService.GetDetails(id);

            var userId = this.User.GetId();

            if (insuranceToEdit.UserId != userId)
            {
                return BadRequest();
            }

            var editedInsurance = this.insuranceService.Edit(
                id,
                insurance.StartDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                insurance.EndDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                insurance.NumberOfPeople,
                insurance.Limit,
                insurance.Beneficiary);                
                

            if (!editedInsurance)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = $"Your insurance was edited!";

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var userId = this.User.GetId();

            var insuranceToDelete = this.insuranceService.GetDetails(id);

            if (insuranceToDelete.UserId != userId && !User.IsAdmin())
            {                
                return this.Unauthorized();                
            }

            if (insuranceToDelete.EndDate > DateTime.UtcNow)
            {
                TempData[GlobalMessageKey] = $"You can not delete this insurance as it is not expired yet!";

                return RedirectToAction(nameof(Details), new { id });
            }


            this.insuranceService.Delete(id);

            return RedirectToAction(nameof(MyInsurances));
           
        }

    }
}
