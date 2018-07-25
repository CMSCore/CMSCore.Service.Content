namespace CMSCore.Service.Content.Controllers
{
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/page")]
    public class PageController : Controller
    {
        private readonly IClusterClient _client;

        public PageController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(name);
            var result = await grain.GetPageByNormalizedName();
            return Json(result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(id);
            var result = await grain.GetPageById();
            return Json(result);
        }
    }
}