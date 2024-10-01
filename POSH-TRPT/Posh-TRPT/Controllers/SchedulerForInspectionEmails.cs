using Microsoft.AspNetCore.Mvc;
using Posh_TRPT_Services.InspectionExpiryEmail;
using Quartz;

namespace Posh_TRPT.Controllers
{
    public class EmailShceduler : IJob
    {
        private readonly ExpireInspectionEmailsService _service;
        private readonly ILogger<ExpireInspectionEmailsService> _logger;
        public EmailShceduler(ExpireInspectionEmailsService service, ILogger<ExpireInspectionEmailsService> _logger)
        {
            this._logger =
                _logger;
            _service = service;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Inside EmailShceduler method of EmailShceduler Controller ---{0}", DateTime.UtcNow);
            var data =  _service.SendEmailForInspection().Result;
            return Task.CompletedTask;
        }
    }
}
