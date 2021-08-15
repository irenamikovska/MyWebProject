using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Events;
using WalksInNature.Services.Events;

namespace WalksInNature.Areas.Admin.Controllers
{
    public class EventsController : AdminController
    {
        private readonly IEventService eventService;
        public EventsController(IEventService eventService) => this.eventService = eventService;      

        public IActionResult All([FromQuery] AllEventsQueryModel query)
        {
            var queryResult = this.eventService.All(
                query.GuideName,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllEventsQueryModel.EventsPerPageAdm,
                publicOnly: false);

            var eventGuides = this.eventService.AllEventGuides();

            query.Guides = eventGuides;
            query.TotalEvents = queryResult.TotalEvents;
            query.Events = queryResult.Events;

            return View(query);
        }

        public IActionResult ChangeStatus(int id)
        {
            this.eventService.ChangeStatus(id);

            return RedirectToAction(nameof(All));
        }
               
        public IActionResult Delete(int id)
        {
            
            this.eventService.DeleteByAdmin(id);

            return RedirectToAction(nameof(All));
        }
    }
}
