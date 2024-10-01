using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Models.DTO.MasterTableDTO
{
    public  class CategoryPriceDTO
    {

        public Guid Id { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Cancel_Penalty { get; set; }
        public decimal Cost_Per_Mile { get; set; }
        public decimal Cost_Per_Minute { get; set; }
        public decimal Maximum_Fare { get; set; }
        public decimal Minimum_Fare { get; set; }
        public decimal Sched_Cancel_Penalty { get; set; }
        public decimal Sched_Ride_Minimum_Fare { get; set; }
        public decimal Service_Fee { get; set; }
        public decimal Toll_Fares { get; set; }
        public decimal Airport_Fees { get; set; }

        public decimal Bonus_Amount { get; set; }

        public Guid? RideCategoryId { get; set; }
        public  RideCategoryDTO? RideCategory { get; set; }


        public Guid? StateId { get; set; }
        public  StateDTO? State { get; set; }
        public Guid? CityId { get; set; }
        public CityDTO? City { get; set; }
    }
}
