using Posh_TRPT_Domain.Base;
using Posh_TRPT_Domain.Register;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Entity
{
    public class BookingDetail : AuditEntity<Guid>
    {
      
        public string? DriverId { get; set; }      


        public string? RiderId { get; set; }
        

        [ForeignKey("CategoryPrice")]
        public Guid CategoryPriceId { get; set; }
       
        public virtual CategoryPrice? CategoryPrice { get; set; }

        public string? RiderSourceName { get;set; }
        public double? RiderLat { get; set; }
        public double? RiderLong { get; set; }
        public double? DriverLat { get; set; }
        public double? DriverLong { get; set; }

        [ForeignKey("VehicleDetail")]
        public Guid VehicleId { get; set; }
        public virtual VehicleDetail? VehicleDetail { get; set; }
        public double? Angle { get; set; } = 0;
        public Guid? BookingStatusId { get; set; }
        public string? Price { get; set; }
        public string? Distance { get; set; }
        public string? Time { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? DestinationPlaceName { get; set; }
        public string? RiderDeviceId { get; set; }
        public double? RiderDestinationLat { get; set; }
        public double? RiderDestinationLong { get; set; }
        public string? PaymentIntentId { get; set; }
		public string? DefaultPaymentMethodId { get; set; }
		public string? PaymentStatus { get; set; }
		public int? MinimumDistance { get; set; }
        public string? StatusType { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? StripeFees { get; set; } = 0.0m;
        public decimal? TollFees { get; set; } = 0.0m;
        
        public decimal? Promotion { get; set; } = 0.0m;
        public int DriverReachedStatus { get; set; }= 0;
        public bool iscancelled { get; set; }= false;
        public decimal CancelledCharge { get; set; }= 0.0m;
        public decimal CancelledStripeFee { get; set; }= 0.0m;
        public decimal RiderDriverDistance { get; set; }= 0.0m;
        public DateTime? LocalCreatedDateTime { get; set; }
        public DateTime? LocalUpdatedDateTime { get; set; }
        public DateTime? PickUpTime { get; set; }
        public DateTime? DropTime { get; set; }

    }

}
