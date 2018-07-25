namespace CMSCore.Service.Content.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    public class ContentController : Controller
    {
        private readonly IClusterClient _client;

        public ContentController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpGet("pagetree")]
        public async Task<IActionResult> GetPageTree()
        {
            var grain = this._client.GetGrain<IReadContentGrain>(Guid.Empty.ToString());
            var result = await grain.GetPageTree();
            return Json(result);
        }

        [HttpGet("page/{name}")]
        public async Task<IActionResult> GetPage(string name)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(name);
            var result = await grain.GetPageByNormalizedName();
            return Json(result);
        }

        [HttpGet("page/id/{id}")]
        public async Task<IActionResult> GetPageById(string id)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(id);
            var result = await grain.GetPageById();
            return Json(result);
        }

        [HttpGet("feeditem/id/{id}")]
        public async Task<IActionResult> GetFeedItemById(string id)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(id);
            var result = await grain.GetFeedItem();
            return Json(result);
        }

        [HttpGet("feeditem/{name}")]
        public async Task<IActionResult> GetFeedItem(string name)
        {
            var grain = this._client.GetGrain<IReadContentGrain>(name);
            var result = await grain.GetFeedItemByNormalizedName();
            return Json(result);
        }
    }
}