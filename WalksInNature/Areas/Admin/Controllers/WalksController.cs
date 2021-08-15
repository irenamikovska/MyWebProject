using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Walks;
using WalksInNature.Services.Walks;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class WalksController : AdminController
    {
        private readonly IWalkService walkService;
        public WalksController(IWalkService walkService) => this.walkService = walkService;        

        public IActionResult All([FromQuery] AllWalksQueryModel query)
        {
            var queryResult = this.walkService.All(
                    query.Region,
                    query.SearchTerm,
                    query.Sorting,
                    query.CurrentPage,
                    AllWalksQueryModel.WalksPerPageAdm,
                    publicOnly: false);

            var walkRegions = this.walkService.AllWalkRegions();

            query.Regions = walkRegions;
            query.TotalWalks = queryResult.TotalWalks;
            query.Walks = queryResult.Walks;

            return View(query);
        }

        public IActionResult ChangeStatus(int id)
        {
            this.walkService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            
            this.walkService.DeleteByAdmin(id);

            return RedirectToAction(nameof(All));
        }
        
    }
}
