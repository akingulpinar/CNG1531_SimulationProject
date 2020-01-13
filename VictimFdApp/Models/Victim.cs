using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAVProject.Som;

namespace UAVProject.Models
{
    public class Victim
    {
        public VictimStatus Status;
        public Coordinate Location;
        public string VictimDeviceName;

        public Victim()
        {
            Status = VictimStatus.NotDamaged;
            Location = new Coordinate
            {
                Longitude = 100,
                Latitude = 150
            };
            VictimDeviceName = "VictimFederate" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
