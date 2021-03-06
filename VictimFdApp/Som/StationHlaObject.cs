// **************************************************************************************************
//		CStationHlaObject
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.2
//			at		: 	Sunday, November 17, 2019 3:36:15 PM
//		compatible with		: 	RACoN v.0.0.2.5
//
//		copyright		: 	(C) 
//		email			: 	
// **************************************************************************************************
/// <summary>
/// This is a wrapper class for local data structures. This class is extended from the object model of RACoN API
/// </summary>

// System
// Racon
using Racon.RtiLayer;
using UAVProject.Models;
// Application


namespace UAVProject.Som
{
    public class CStationHlaObject : HlaObject
    {
        #region Declarations
        public Station station;
        #endregion //Declarations

        #region Constructor
        public CStationHlaObject(HlaObjectClass _type) : base(_type)
        {
            station = new Station();
        }
        // Copy constructor - used in callbacks
        public CStationHlaObject(HlaObject _obj) : base(_obj)
        {
            station = new Station();
        }
        #endregion //Constructor
    }
}
