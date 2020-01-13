// **************************************************************************************************
//		CUAVOC
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
  public class CUAVOC : HlaObjectClass
  {
    #region Declarations
    public HlaAttribute UAVName;
    public HlaAttribute LeftBattery;
    public HlaAttribute Speed;
    public HlaAttribute UAVType;
    public HlaAttribute VictimsFound;
    public HlaAttribute Position;
    public HlaAttribute VictimList;
    public HlaAttribute TargetPosition;
    public HlaAttribute Radius;
    #endregion //Declarations
    
    #region Constructor
    public CUAVOC() : base()
    {
      // Initialize Class Properties
      Name = "HLAobjectRoot.UAV";
      ClassPS = PSKind.PublishSubscribe;
      
      // Create Attributes
      // UAVName
      UAVName = new HlaAttribute("UAVName", PSKind.Subscribe);
      Attributes.Add(UAVName);
      // LeftBattery
      LeftBattery = new HlaAttribute("LeftBattery", PSKind.Neither);
      Attributes.Add(LeftBattery);
      // Speed
      Speed = new HlaAttribute("Speed", PSKind.Neither);
      Attributes.Add(Speed);
      // UAVType
      UAVType = new HlaAttribute("UAVType", PSKind.Neither);
      Attributes.Add(UAVType);
      // VictimsFound
      VictimsFound = new HlaAttribute("VictimsFound", PSKind.Neither);
      Attributes.Add(VictimsFound);
      // Position
      Position = new HlaAttribute("Position", PSKind.Subscribe);
      Attributes.Add(Position);
      // VictimList
      VictimList = new HlaAttribute("VictimList", PSKind.Neither);
      Attributes.Add(VictimList);
      // TargetPosition
      TargetPosition = new HlaAttribute("TargetPosition", PSKind.Neither);
      Attributes.Add(TargetPosition);
      // Radius
      Radius = new HlaAttribute("Radius", PSKind.Neither);
      Attributes.Add(Radius);
    }
    #endregion //Constructor
  }
}
