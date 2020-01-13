using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAVProject.Som;

namespace UAVProject.Models
{
    public class Station
    {
        public Coordinate Location;
        public string StationName;

        public Station()
        {
            Location = new Coordinate
            {
                Longitude = 0,
                Latitude = 0
            };
            StationName = "";
        }
    }
}
