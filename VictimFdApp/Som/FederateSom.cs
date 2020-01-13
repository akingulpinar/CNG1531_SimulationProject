// **************************************************************************************************
//		FederateSom
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
  public class FederateSom : Racon.ObjectModel.CObjectModel
  {
    #region Declarations
    #region SOM Declaration
    public UAVProject.Som.CUAVOC UAVOC;
    public UAVProject.Som.CVictimOC VictimOC;
    public UAVProject.Som.CStationOC StationOC;
    public UAVProject.Som.CVictimContactIC VictimContactIC;
    public UAVProject.Som.CUAVCommsIC UAVCommsIC;
    public UAVProject.Som.CStationCommsIC StationCommsIC;
    #endregion
    #endregion //Declarations
    
    #region Constructor
    public FederateSom() : base()
    {
      // Construct SOM
      UAVOC = new UAVProject.Som.CUAVOC();
      AddToObjectModel(UAVOC);
      VictimOC = new UAVProject.Som.CVictimOC();
      AddToObjectModel(VictimOC);
      StationOC = new UAVProject.Som.CStationOC();
      AddToObjectModel(StationOC);
      VictimContactIC = new UAVProject.Som.CVictimContactIC();
      AddToObjectModel(VictimContactIC);
      UAVCommsIC = new UAVProject.Som.CUAVCommsIC();
      AddToObjectModel(UAVCommsIC);
      StationCommsIC = new UAVProject.Som.CStationCommsIC();
      AddToObjectModel(StationCommsIC);
    }
    #endregion //Constructor
  }
}
