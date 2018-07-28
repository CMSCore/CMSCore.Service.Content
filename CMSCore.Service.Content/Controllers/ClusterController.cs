namespace CMSCore.Service.Content.Controllers
{
    using Library.Core.Service;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class ClusterController : ClusterControllerBase
    {
        public ClusterController(IClusterClient client) : base(client)
        {
        }
    }
}