using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Domain.Contracts;
using ServiceMonitor.Web.Models;

namespace ServiceMonitor.Web.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IStatusLogService _storeService;

        public HomeController(IStatusLogService storeService)
        {
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new HomeViewModel() { StatusStatistics = await _storeService.GetStatusStatisticsAsync() });
        }
    }
}