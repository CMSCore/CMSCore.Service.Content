namespace CMSCore.Service.Content.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Library.Core.Attributes;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class FeedItemController : Controller
    {
        private readonly IClusterClient _client;

        public FeedItemController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpGet("{name}")]
        [ValidateModel]
        public async Task<IActionResult> Get([Required] string name)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(name);
            var result = await grain.GetFeedItemByNormalizedName();
            var value = result.Value;
            return Json(value);
        }

        [HttpGet("id/{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetById(string id)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(id);
            var result = await grain.GetFeedItem();
            var value = result.Value;
            return Json(value);
        }
    }
}