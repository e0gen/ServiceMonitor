using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.Domain.Models;

namespace ServiceMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    [Controller]
    public class StatusController : ControllerBase
    {
        private readonly IStatusLogService _storeService;

        public StatusController(IStatusLogService storeService)
        {
            _storeService = storeService;
        }
        private bool state = true;
        [HttpGet]
        public IActionResult GetStatusAsync()
        {
            if(state)
                return Ok();

            return BadRequest();
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendStatusAsync([FromBody]StatusLog sl)
        {
            _storeService.AddStatusLog(sl);
            await _storeService.SaveChangesAsync();

            return Ok();
        }
    }
}