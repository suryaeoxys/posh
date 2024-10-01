using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class UserApprovalStatus
    {
        public Guid? StatusId { get; set; }
        public Guid? RideCategoryId { get; set; }
        public string? Message { get; set; }
        public string? UserId { get; set; }
    }
}
