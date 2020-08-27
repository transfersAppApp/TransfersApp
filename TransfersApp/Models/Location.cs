using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransfersApp.Models
{
    public class Locations
    {
        public List<Location> Results { get; set; }
    }

    public class Location
    { 
        public Coordinates geometry { get; set; }
        public Country components { get; set; }
    }

    public class Country
    {
        public string country_code { get; set; }
    }
    public class Coordinates
    { 
        public string lat { get; set; }
        public string lng { get; set; }
    }
}