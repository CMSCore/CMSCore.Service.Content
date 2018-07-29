namespace CMSCore.Service.Content.Controllers
{
    using System;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    public abstract class GrainController : Controller
    {
        private readonly IClusterClient _client;

        protected GrainController(IClusterClient client)
        {
            this._client = client;
        }


        [HttpGet("grainidentity")]
        public virtual IActionResult GrainIdentity()
        {
            var result = _client.GetGrain<IReadContentGrain>(Guid.NewGuid().ToString()).GetGrainIdentity();
            return Json(result);
        }
    }
}