using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.InspectionData
{
    public interface IExpireInspectionEmails
    {
        Task<bool> SendEmailToDriverAndADMIN();
    }
}
