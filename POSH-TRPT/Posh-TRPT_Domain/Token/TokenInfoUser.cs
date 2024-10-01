using Posh_TRPT_Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Token
{
    [Table("TblTokenInfoUsers")]
    public class TokenInfoUser: IDeleteEntity, IAuditEntity
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? RefreshToken { get; set; }
        [NotMapped]
        public string? AccessToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public bool IsDeleted { get ; set; }
        public DateTime? CreatedDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? CreatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? UpdatedBy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime? UpdatedDate { get; set; }
    }
}
