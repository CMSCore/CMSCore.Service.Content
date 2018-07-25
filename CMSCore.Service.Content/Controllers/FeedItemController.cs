namespace CMSCore.Service.Content.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/page")]
    public class FeedItemController : Controller
    {
        private readonly IClusterClient _client;

        public FeedItemController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(name);
            var result = await grain.GetFeedItemByNormalizedName();
            return Json(result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(id);
            var result = await grain.GetFeedItem();
            return Json(result);
        }
    }
}