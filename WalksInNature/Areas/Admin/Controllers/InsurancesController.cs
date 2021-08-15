using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Insurances;
using WalksInNature.Services.Insurances;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class InsurancesController : AdminController
    {
        private readonly IInsuranceService insuranceService;
        public InsurancesController(IInsuranceService insuranceService) => this.insuranceService = insuranceService;
       
        public IActionResult All([FromQuery] AllInsuranceQueryModel query)
        {
            var queryResult = this.insuranceService.All(
                query.UserId,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllInsuranceQueryModel.InsurancesPerPage);

            var userIds = this.insuranceService.AllUserId();

            query.UserIds = userIds;
            query.TotalInsurance = queryResult.TotalInsurances;
            query.Insurances = queryResult.Insurances;

            return View(query);
        }

        public IActionResult ChangeStatus(string id)
        {
            this.insuranceService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }                      
       
        public IActionResult Delete(string id)
        {

            this.insuranceService.DeleteByAdmin(id);

            return RedirectToAction(nameof(All));
        }
    }
}
