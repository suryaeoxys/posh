using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Posh_TRPT_Domain.DashBoard;
using Posh_TRPT_Domain.Entity;
using Posh_TRPT_Domain.InspectionData;
using Posh_TRPT_Models.DTO.ExpireInspection;
using Posh_TRPT_Utility.EmailUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Infrastructure.Repositories
{
    public class ExpireInspectionEmailsRepository : Repository<ApplicationUser>, IExpireInspectionEmails
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingSystemRepository> _logger;
        public static HttpContext _httpContext => new HttpContextAccessor().HttpContext!;
        public ExpireInspectionEmailsRepository(IConfiguration config, UserManager<ApplicationUser> userManager, ILogger<BookingSystemRepository> logger, DbFactory dbFactory) : base(dbFactory)
        {
            _config = config;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> SendEmailToDriverAndADMIN()
        {
            List<DueInspectionsDriversData> driverdata = new List<DueInspectionsDriversData>();
            
            _logger.LogInformation("{0} InSide before  SendEmailToDriverAndADMIN in ExpireInspectionEmailsRepository Method ", DateTime.UtcNow);
            driverdata = await this.DbContextObj().GetListOfRecordExecuteProcedureAsync<DueInspectionsDriversData>("Sp_GetDriversWithInspectionDueSoon", new SqlParameter[] { });
                foreach (var item in driverdata)
                {
                    EmailToDriverDueInspection email = new EmailToDriverDueInspection()
                    {
                        DriverName = item.DriverName,
                        MailTo = item.Email,
                        Port = Convert.ToInt32(_config["EmailConfiguration:Port"]),
                        MailFrom = _config["EmailConfiguration:AdminEmail"]!,
                        Password = _config["EmailConfiguration:Password"]!,
                        Host = _config["EmailConfiguration:Host"]!,
                        Subject = _config["EmailConfiguration:InspectionSubject"]!,
                        MailFromAlias = _config["EmailConfiguration:Alias"]!,
                        InspectionNote = item.InspectionNote,
                        Inspection_Expiry_Date = item.Inspection_Expiry_Date?.Date.ToString("MM/dd/yyyy"),
                        EmailCC = _config["EmailConfiguration:AdminEmail"]!,
                        PhoneNumber = item.PhoneNumber
                    };
                    Parallel.Invoke(() =>  EmailUtility.SendEmailToDriverExpireInspection(email));
                _logger.LogInformation("{0} InSide after calling SP Sp_GetDriversWithInspectionDueSoon SendEmailToDriverAndADMIN in ExpireInspectionEmailsRepository Method -- =", DateTime.UtcNow);
            }
            return true;
        }
    }
}
