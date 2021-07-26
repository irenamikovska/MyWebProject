using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Guides;
using WalksInNature.Services.Guides;

namespace WalksInNature.Controllers
{
    public class GuidesController : Controller
    {
        private readonly IGuideService guideService;
        public GuidesController(IGuideService guideService)
            => this.guideService = guideService;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeGuideFormModel input)
        {
            var userId = this.User.GetId();
                        
            var userIsAlreadyGuide = this.guideService.IsGuide(userId);

            if (userIsAlreadyGuide)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }
           
            this.guideService.Create(input.Name, input.PhoneNumber, userId);
           
            return RedirectToAction("All", "Events");
        }
    }
}
