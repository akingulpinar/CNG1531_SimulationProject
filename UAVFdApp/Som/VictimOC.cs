// **************************************************************************************************
//		CVictimOC
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
  public class CVictimOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute VictimDeviceName;
    public HlaAttribute Location;
    public HlaAttribute Status;
    #endregion //Declarations
    
    #region Constructor
    public CVictimOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.Victim";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Attributes
      // VictimDeviceName
      VictimDeviceName = new HlaAttribute("VictimDeviceName", PSKind.Subscribe);
      Attributes.Add(VictimDeviceName);
      // Location
      Location = new HlaAttribute("Location", PSKind.Subscribe);
      Attributes.Add(Location);
      // Status
      Status = new HlaAttribute("Status", PSKind.Subscribe);
      Attributes.Add(Status);
    }
    #endregion //Constructor
  }
}
