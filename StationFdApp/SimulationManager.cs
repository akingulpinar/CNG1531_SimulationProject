// **************************************************************************************************
//		CSimulationManager
//
//		generated
//			by		: 	Simulation Generator (SimGe) v.0.3.2
//			at		: 	Thursday, December 19, 2019 5:43:07 PM
//		compatible with		: 	RACoN v.0.0.2.5
//
//		copyright		: 	(C) 
//		email			: 	
// **************************************************************************************************
/// <summary>
/// The Simulation Manager manages the (multiple) federation execution(s) and the (multiple instances of) joined federate(s).
/// </summary>

// System
// Racon
using Racon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
// Application
using UAVProject.Som;

namespace UAVProject
{
    public class CSimulationManager
    {
        #region Declarations
        // Communication layer related structures
        public CStationFdApp federate; //Application-specific federate 
        public BindingList<Tuple<int,CUAVHlaObject>> UAVObjects;
        public BindingList<CStationHlaObject> StationObjects;
        public BindingList<Tuple<int, CVictimHlaObject>> VictimObjects;
        public int numberOfFoundVictims = 0;
        public List<string> foundVictimNames;
        public bool IsRadioOn = false;
        public MainWindow window;
        #endregion //Declarations

        #region Constructor
        public CSimulationManager(MainWindow w)
        {
            window = w;
            // Initialize the application-specific federate
            federate = new CStationFdApp(this);
            // Initialize the federation execution
            federate.FederationExecution.Name = "UAVDisasterFederationExecution";
            federate.FederationExecution.FederateType = "StationFederateType";
            federate.FederationExecution.ConnectionSettings = "rti://10.144.31.177";
            // Handle RTI type variation

            UAVObjects = new BindingList<Tuple<int, CUAVHlaObject>>();
            StationObjects = new BindingList<CStationHlaObject>();
            VictimObjects = new BindingList<Tuple<int, CVictimHlaObject>>();

            initialize();

            federate.LogLevel = Racon.LogLevel.ALL;
            federate.FederateStateChanged += Federate_FederateStateChanged;
            federate.InteractionReceived += federate.FdAmb_InteractionReceivedHandler;

            foundVictimNames = new List<string>();
        }
        #endregion //Constructor

        #region Methods
        // Handles naming variation according to HLA specification
        private void initialize()
        {
            switch (federate.RTILibrary)
            {
                case RTILibraryType.HLA13_DMSO:
                case RTILibraryType.HLA13_Portico:
                case RTILibraryType.HLA13_OpenRti:
                    federate.Som.UAVOC.Name = "objectRoot.UAV";
                    federate.Som.UAVOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    federate.Som.VictimOC.Name = "objectRoot.Victim";
                    federate.Som.VictimOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    federate.Som.StationOC.Name = "objectRoot.Station";
                    federate.Som.StationOC.PrivilegeToDelete.Name = "privilegeToDelete";
                    federate.Som.VictimContactIC.Name = "interactionRoot.VictimContact";
                    federate.Som.UAVCommsIC.Name = "interactionRoot.UAVComms";
                    federate.Som.StationCommsIC.Name = "interactionRoot.StationComms";
                    federate.FederationExecution.FDD = @".\UAVFOM.fed";
                    break;
                case RTILibraryType.HLA1516e_Portico:
                case RTILibraryType.HLA1516e_OpenRti:
                    federate.Som.UAVOC.Name = "HLAobjectRoot.UAV";
                    federate.Som.UAVOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    federate.Som.VictimOC.Name = "HLAobjectRoot.Victim";
                    federate.Som.VictimOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    federate.Som.StationOC.Name = "HLAobjectRoot.Station";
                    federate.Som.StationOC.PrivilegeToDelete.Name = "HLAprivilegeToDeleteObject";
                    federate.Som.VictimContactIC.Name = "HLAinteractionRoot.VictimContact";
                    federate.Som.UAVCommsIC.Name = "HLAinteractionRoot.UAVComms";
                    federate.Som.StationCommsIC.Name = "HLAinteractionRoot.StationComms";
                    federate.FederationExecution.FDD = @".\UAVFOM.xml";
                    break;
            }
        }

        public void BeginSimulation()
        {
            federate.Connect(callbackModel: Racon.CallbackModel.EVOKED, "");
            federate.CreateFederationExecution("UAVDisasterFederationExecution", "UAVFOM.xml", "");
            federate.JoinFederationExecution("UAVFederate" + Guid.NewGuid(), "StationFederateType", "UAVDisasterFederationExecution");

            federate.DeclareCapability();

            #region ClassObjects
            CStationHlaObject stationObject = new CStationHlaObject(federate.Som.StationOC);
            stationObject.station = new Models.Station();
            StationObjects.Add(stationObject);
            #endregion

            //federate.ResignFederationExecution(action: ResignAction.CANCEL_PENDING_OWNERSHIP_ACQUISITIONS);
            //federate.DestroyFederationExecution("UAVFederation");
            //federate.Disconnect();
        }

        private void Federate_FederateStateChanged(object sender, CFederateStateEventArgs e)
        {
            switch (e.FdState)
            {
                case Racon.FederateStates.NOTCONNECTED:
                    Console.WriteLine("NOT CONNECTED");
                    break;
                case Racon.FederateStates.CONNECTED | Racon.FederateStates.JOINED | Racon.FederateStates.FREERUN:
                    Console.WriteLine("CONNECTED, JOINED, FREERUN");
                    break;
                default:
                    break;
            }
        }
        #endregion //Methods
    }
}
