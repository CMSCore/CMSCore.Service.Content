namespace CMSCore.Service.Content.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/metadata")]
    public class MetadataController : Controller
    {
        private readonly IClusterClient _client;

        public MetadataController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpGet("links")]
        public async Task<IActionResult> Links()
        {
            var grain = this._client.GetGrain<IReadContentGrain>(Guid.Empty.ToString());
            var result = await grain.GetPageTree();
            return Json(result);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> Tags()
        {
            var grain = this._client.GetGrain<IReadContentGrain>(Guid.Empty.ToString());
            var result = await grain.GetTags();
            return Json(result);
        }
    }
}