using Microsoft.AspNetCore.Mvc;
using WalksInNature.Services.Events;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class EventsController : AdminController
    {
        private readonly IEventService eventService;
        public EventsController(IEventService eventService) => this.eventService = eventService;

        public IActionResult All()
        {
            var events = this.eventService
                .All(publicOnly: false)
                .Events;

            return View(events);
        }

        public IActionResult ChangeStatus(int id)
        {
            this.eventService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }
    }
}
