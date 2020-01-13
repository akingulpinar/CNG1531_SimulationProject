using System.Collections.Generic;
using UAVProject.Som;

namespace UAVProject.Models
{
    public class UAV
    {
        public string UAVName;
        public string UAVType;
        public int Speed;
        public float LeftBattery;
        public bool VictimsFound;
        public Coordinate Position;
        public List<string> VictimList;
        public Coordinate TargetPosition;
        public int Radius;

        public UAV()
        {
            UAVName = "";
            UAVType = "";
            Speed = 0;
            LeftBattery = 100;
            VictimsFound = false;
            Position = new Coordinate
            {
                Longitude = 0,
                Latitude = 0
            };
            VictimList = new List<string>();
            TargetPosition = new Coordinate
            {
                Longitude = 0,
                Latitude = 0
            };
            Radius = 0;
        }
    }
}
