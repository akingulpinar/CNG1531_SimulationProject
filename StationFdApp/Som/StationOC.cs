// **************************************************************************************************
//		CStationOC
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
/// This class is extended from the object model of RACoN API
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
// Racon
using Racon;
using Racon.RtiLayer;
// Application
using UAVProject.Som;


namespace UAVProject.Som
{
  public class CStationOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute StationName;
    public HlaAttribute Location;
    #endregion //Declarations
    
    #region Constructor
    public CStationOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.Station";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Attributes
      // StationName
      StationName = new HlaAttribute("StationName", PSKind.PublishSubscribe);
      Attributes.Add(StationName);
      // Location
      Location = new HlaAttribute("Location", PSKind.PublishSubscribe);
      Attributes.Add(Location);
    }
    #endregion //Constructor
  }
}
