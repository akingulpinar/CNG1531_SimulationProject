// **************************************************************************************************
//		CUAVCommsIC
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
  public class CUAVCommsIC : HlaInteractionClass
  {
    #region Declarations
    public HlaParameter VictimList;
    public HlaParameter UAVName;
    public HlaParameter ServedVictims;
    public HlaParameter Timestamp;
    #endregion //Declarations
    
    #region Constructor
    public CUAVCommsIC() : base()
    {
      // Initialize Class Properties
      Name = "HLAinteractionRoot.UAVComms";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Parameters
      // VictimList
      VictimList = new HlaParameter("VictimList");
      Parameters.Add(VictimList);
      // UAVName
      UAVName = new HlaParameter("UAVName");
      Parameters.Add(UAVName);
      // ServedVictims
      ServedVictims = new HlaParameter("ServedVictims");
      Parameters.Add(ServedVictims);
      // Timestamp
      Timestamp = new HlaParameter("Timestamp");
      Parameters.Add(Timestamp);
    }
    #endregion //Constructor
  }
}
