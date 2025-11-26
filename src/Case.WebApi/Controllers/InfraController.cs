using Microsoft.AspNetCore.Mvc;

namespace Case.WebApi.Controllers
{
    [Route("api/infra")]
    [Produces("application/json")]
    public class InfraController : ControllerBase
    {
        public InfraController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}