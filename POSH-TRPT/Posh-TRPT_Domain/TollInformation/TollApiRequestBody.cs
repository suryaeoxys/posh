namespace Posh_TRPT_Domain.TollApiRequestBody
{
    public class TollApiRequestBody
    {
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public string travelMode { get; set; }
        public List<string> extraComputations { get; set; }
    }

    public class Destination
    {
        public Location location { get; set; }
    }

    public class LatLng
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Location
    {
        public LatLng latLng { get; set; }
    }

    public class Origin
    {
        public Location location { get; set; }
    }

}
