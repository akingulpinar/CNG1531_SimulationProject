// **************************************************************************************************
//		CVictimContactIC
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
  public class CVictimContactIC : HlaInteractionClass
  {
    #region Declarations
    public HlaParameter StatusMessage;
    public HlaParameter Timestamp;
    #endregion //Declarations
    
    #region Constructor
    public CVictimContactIC() : base()
    {
      // Initialize Class Properties
      Name = "HLAinteractionRoot.VictimContact";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Parameters
      // StatusMessage
      StatusMessage = new HlaParameter("StatusMessage");
      Parameters.Add(StatusMessage);
      // Timestamp
      Timestamp = new HlaParameter("Timestamp");
      Parameters.Add(Timestamp);
    }
    #endregion //Constructor
  }
}
