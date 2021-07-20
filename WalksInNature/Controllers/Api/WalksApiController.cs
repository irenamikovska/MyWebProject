using Microsoft.AspNetCore.Mvc;
using WalksInNature.Models.Api.Walks;
using WalksInNature.Services.Walks;

namespace WalksInNature.Controllers.Api
{

    [ApiController]
    [Route("api/walks")]
    public class WalksApiController : ControllerBase
    {        
        private readonly IWalkService walks;
        public WalksApiController(IWalkService walks)
            => this.walks = walks;

        [HttpGet]
        public WalkQueryServiceModel All([FromQuery] AllWalksApiRequestModel query)
            => this.walks.All(query.Region,
                              query.SearchTerm,
                              query.Sorting,
                              query.CurrentPage,
                              query.WalksPerPage);
        
    }
}
