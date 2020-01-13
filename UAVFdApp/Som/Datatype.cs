// **************************************************************************************************
//		Data Types
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
/// This file includes the enumerated and fixed record data types.
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
  #region Enumerated Datatypes
  public enum VictimStatus {  NotDamaged = 0, SlightlyDamaged = 1, SeverelyDamaged = 2, CriticallyDamaged = 3, Served = 4 };
  #endregion
  
  #region Fixed Record Datatypes
  public struct Coordinate
  {
    public double Longitude; // Datatype defined in SOM: HLAfloat64Time
    public double Latitude; // Datatype defined in SOM: HLAfloat64Time
  }
    
  #endregion
  #region Variant Record Datatypes
  #endregion
  
}
