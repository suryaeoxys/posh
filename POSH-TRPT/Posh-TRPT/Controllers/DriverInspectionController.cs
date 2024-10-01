using Microsoft.AspNetCore.Mvc;

namespace Posh_TRPT.Controllers
{
    public class DriverInspectionController : Controller
    {
        private readonly ILogger<DriverInspectionController> _logger;
        private readonly IConfiguration _configuration;
        public DriverInspectionController(ILogger<DriverInspectionController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
