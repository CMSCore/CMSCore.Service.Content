namespace CMSCore.Service.Content.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;
    using Orleans.Configuration.Overrides;

    [Route("api/v1/cluster")]
    public class ClusterController : Controller
    {
        private readonly IClusterClient _client;

        public ClusterController(IClusterClient client)
        {
            _client = client;
        }

        [HttpGet("grain/identity")]
        public virtual IActionResult Identity()
        {
            var grain = _client.GetGrain<IReadContentGrain>(Guid.Empty.ToString());
            var identity = grain.GetGrainIdentity();
            return Json(identity);
        }

        [HttpPost("connection/connect")]
        public virtual async Task<IActionResult> Connect()
        {
            try
            {
                await _client.Connect();

                return Json(new
                {
                    _client.IsInitialized
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("connection/close")]
        public virtual async Task<IActionResult> Close()
        {
            try
            {
                await _client.Close();
 
                return Json(new
                {
                    _client.IsInitialized
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("options/{providerName}")]
        public virtual IActionResult Options(string providerName)
        {
            try
            {
                var options = _client.ServiceProvider.GetProviderClusterOptions(providerName);
                return Json(options);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}