using Posh_TRPT_Models.DTO;
using Posh_TRPT_Services.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Utility.Resources;
using Posh_TRPT_Utility.ConstantStrings;

namespace Posh_TRPT.Controllers
{
    [Route(GlobalConstants.GlobalValues.ControllerRoute)]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(EmployeeService service, ILogger<EmployeeController> logger)
        {
            _service = service;
            _logger = logger;
        }
        /// <summary>
        /// API to get Emaployee details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            _logger.LogInformation(Configuration.EmployeeAccessed);

            var users = await _service.GetUsers();

            return Ok(users);
        }
        /// <summary>
        /// API to add employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(EmployeeDTO model)
        {
            var users = await _service.AddUser(model);

            return Ok(users);
        }
    }
}
