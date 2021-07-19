using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WalksInNature.Data;
using WalksInNature.Data.Models;
using WalksInNature.Infrastructure;
using WalksInNature.Models.Guides;

namespace WalksInNature.Controllers
{
    public class GuidesController : Controller
    {
        private readonly WalksDbContext data;
        public GuidesController(WalksDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeGuideFormModel input)
        {
            var userId = this.User.GetId();

            var userIsAlreadyGuide = this.data
                .Guides
                .Any(d => d.UserId == userId);

            if (userIsAlreadyGuide)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var guideToAdd = new Guide
            {
                Name = input.Name,
                PhoneNumber = input.PhoneNumber,
                UserId = userId
            };

            this.data.Guides.Add(guideToAdd);
            this.data.SaveChanges();

            return RedirectToAction("All", "Events");
        }
    }
}
