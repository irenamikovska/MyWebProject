using Microsoft.AspNetCore.Mvc;
using WalksInNature.Services.Walks;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class WalksController : AdminController
    {
        private readonly IWalkService walkService;
        public WalksController(IWalkService walkService) => this.walkService = walkService;

        public IActionResult All()
        {
            var walks = this.walkService
                .All(publicOnly: false)
                .Walks;

            return View(walks);
        }

        public IActionResult ChangeStatus(int id)
        {
            this.walkService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            
            this.walkService.Delete(id);

            return RedirectToAction(nameof(All));
        }
    }
}
