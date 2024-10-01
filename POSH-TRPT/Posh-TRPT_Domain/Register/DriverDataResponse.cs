using Posh_TRPT_Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posh_TRPT_Domain.Register
{
    public class DriverDataResponse
    {
        public DriverDataResponse()
        {
            this.CityData = new DriverCityData();
            this.CountryData = new DriverCountryData();
            this.StateData = new DriverStateData();
            this.VehicleData = new DriverVehicleData();
            this.DocumentsData = new DriverDocumentsData();
            this.User = new DriverUserData();
            this.AddressData = new DriverAddressData();
            this.TotalEarnings = 0.0;
            this.Rating = 0.0;
        }
        public double TotalEarnings { get; set; }
        public double Rating { get; set; }
        public DriverCountryData? CountryData { get; set; }
        public DriverCityData? CityData { get; set; }
        public DriverStateData? StateData { get; set; }
        public DriverVehicleData? VehicleData { get; set; }
        public DriverDocumentsData? DocumentsData { get; set; }
        public DriverUserData? User { get; set; }
        public DriverAddressData? AddressData { get; set; }
    }
}
