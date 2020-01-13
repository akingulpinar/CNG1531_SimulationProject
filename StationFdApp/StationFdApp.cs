// **************************************************************************************************
//		CStationFdApp
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
/// The application specific federate that is extended from the Generic Federate Class of RACoN API. This file is intended for manual code operations.
/// </summary>

// System
using System;
using System.Collections.Generic; // for List
using System.Linq;
// Racon
using Racon;
using Racon.RtiLayer;
using UAVProject.Models;
// Application
using UAVProject.Som;

namespace UAVProject
{
    public partial class CStationFdApp : Racon.CGenericFederate
    {
        #region Manually Added Code

        // Local Data
        private CSimulationManager manager;
        private object thisLock = new object();

        #region Constructor
        public CStationFdApp(CSimulationManager parent)
        {
            Som = new UAVProject.Som.FederateSom();
            SetSom(Som);
            manager = parent;
        }
        #endregion //Constructor

        #region Federation Management Callbacks
        public override void FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(sender, data);

            #region User Code
            manager.window.StationOutputBox.Text += "Confirmed sync point!" + Environment.NewLine;
            #endregion //User Code
        }
        public override void FdAmb_OnSynchronizationPointRegistrationFailedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationFailedHandler(sender, data);

            #region User Code
            manager.window.StationOutputBox.Text += "Failed sync point!" + Environment.NewLine;
            #endregion //User Code
        }
        public override void FdAmb_SynchronizationPointAnnounced(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_SynchronizationPointAnnounced(sender, data);

            #region User Code
            bool check = false;
            if (data.Label.Equals("uavReady"))
            {
                manager.window.StationOutputBox.Text += "Checking batteries!!!" + Environment.NewLine;
                foreach (var item in manager.UAVObjects)
                {
                    manager.window.StationOutputBox.Text += item.Item2.uav.UAVName + Environment.NewLine;
                    manager.window.StationOutputBox.Text += item.Item2.uav.LeftBattery + Environment.NewLine;
                    if (item.Item2.uav.LeftBattery > 75)
                    {
                        manager.window.StationOutputBox.Text += item.Item2.uav.UAVName + " is ready." + Environment.NewLine;
                        check = true;
                    }
                    else
                    {
                        check = false;
                        break;
                    }
                }
            }
            else if (data.Label.Equals("allReported"))
            {
                manager.window.StationOutputBox.Text += "All victims are reported to the station!!!" + Environment.NewLine;
                check = true;
            }
            manager.federate.SynchronizationPointAchieved(data.Label, check);

            #endregion //User Code
        }
        public override void FdAmb_FederationSynchronized(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_FederationSynchronized(sender, data);

            #region User Code
            manager.window.StationOutputBox.Text += "Federation synchronized!" + Environment.NewLine;
            #endregion //User Code
        }
        #endregion
        #region Declaration Management Callbacks
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            if (data.ObjectClassHandle == Som.StationOC.Handle)
                RegisterHlaObject(manager.StationObjects[0]);
            #endregion //User Code
        }
        public override void FdAmb_StopRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StopRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            #endregion //User Code
        }
        public override void FdAmb_TurnInteractionsOffAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TurnInteractionsOffAdvisedHandler(sender, data);

            #region User Code
            manager.IsRadioOn = false;
            #endregion //User Code
        }
        public override void FdAmb_TurnInteractionsOnAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TurnInteractionsOnAdvisedHandler(sender, data);

            #region User Code
            manager.IsRadioOn = true;
            #endregion //User Code
        }
        #endregion
        #region Object Management Callbacks
        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            if (data.ClassHandle == Som.StationOC.Handle)
            {
                CStationHlaObject station = new CStationHlaObject(data.ObjectInstance);
                station.Type = Som.StationOC;
                manager.StationObjects.Add(station);

                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.StationOC.Location);
                attributes.Add(Som.StationOC.StationName);
                RequestAttributeValueUpdate(station, attributes, "");
            }
            else if (data.ClassHandle == Som.UAVOC.Handle)
            {
                CUAVHlaObject uav = new CUAVHlaObject(data.ObjectInstance);
                uav.Type = Som.UAVOC;
                manager.UAVObjects.Add(new Tuple<int, CUAVHlaObject>(0, uav));

                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.UAVOC.LeftBattery);
                attributes.Add(Som.UAVOC.Position);
                attributes.Add(Som.UAVOC.Radius);
                attributes.Add(Som.UAVOC.Speed);
                attributes.Add(Som.UAVOC.TargetPosition);
                attributes.Add(Som.UAVOC.UAVName);
                attributes.Add(Som.UAVOC.UAVType);
                attributes.Add(Som.UAVOC.VictimList);
                attributes.Add(Som.UAVOC.VictimsFound);

                RequestAttributeValueUpdate(uav, attributes, "");
            }
            else if (data.ClassHandle == Som.VictimOC.Handle)
            {
                CVictimHlaObject victim = new CVictimHlaObject(data.ObjectInstance);
                victim.Type = Som.VictimOC;
                manager.VictimObjects.Add(new Tuple<int, CVictimHlaObject>(0, victim));

                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.VictimOC.Location);
                attributes.Add(Som.VictimOC.Status);
                attributes.Add(Som.VictimOC.VictimDeviceName);

                RequestAttributeValueUpdate(victim, attributes, "");
            }
            #endregion //User Code
        }
        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            object[] snap;
            lock (thisLock)
            {
                snap = manager.StationObjects.ToArray();
            }
            foreach (CStationHlaObject station in snap)
            {
                if (data.ObjectInstance.Handle == station.Handle)// Find the Object
                {
                    manager.StationObjects.Remove(station);
                    manager.window.StationOutputBox.Text += ($"Station: {station.station.StationName} left. Number of stations Now: {manager.StationObjects.Count}" + Environment.NewLine);
                }
            }
            lock (thisLock)
            {
                snap = manager.UAVObjects.ToArray();
            }
            foreach (Tuple<int, CUAVHlaObject> uav in snap)
            {
                if (data.ObjectInstance.Handle == uav.Item2.Handle)// Find the Object
                {
                    Station.RemoveEntity(manager.window, uav.Item2.uav);
                    manager.UAVObjects.Remove(uav);
                    manager.window.StationOutputBox.Text += ($"UAV: {uav.Item2.uav.UAVName} left. Number of UAVs Now: {manager.UAVObjects.Count}" + Environment.NewLine);
                }
            }
            lock (thisLock)
            {
                snap = manager.VictimObjects.ToArray();
            }
            foreach (Tuple<int, CVictimHlaObject> victim in snap)
            {
                if (data.ObjectInstance.Handle == victim.Item2.Handle)// Find the Object
                {
                    Station.RemoveEntity(manager.window, victim.Item2.victim);
                    manager.VictimObjects.Remove(victim);
                    manager.window.StationOutputBox.Text += ($"Victim: {victim.Item2.victim.VictimDeviceName} left. Number of Victims Now: {manager.VictimObjects.Count}" + Environment.NewLine);
                }
            }

            #endregion //User Code
        }
        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

            #region User Code
            if (data.ObjectInstance.Handle == manager.StationObjects[0].Handle)
            {
                foreach (var item in data.ObjectInstance.Attributes)
                {
                    if (item.Handle == Som.StationOC.StationName.Handle)
                    {
                        UpdateStationName(manager.StationObjects[0]);
                    }
                    else if (item.Handle == Som.StationOC.Location.Handle)
                    {
                        UpdateLocation(manager.StationObjects[0]);
                    }

                    manager.window.StationOutputBox.Text += "Update successful, the object:" + item.Value;
                }
                #endregion //User Code
            }
        }
        public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectAttributesReflectedHandler(sender, data);

            #region User Code
            foreach (var item in manager.StationObjects)
            {
                if (data.ObjectInstance.Handle == item.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.StationOC.Location.Handle) item.station.Location = data.GetAttributeValue<Coordinate>(Som.StationOC.Location);
                        else if (pair.Handle == Som.StationOC.StationName.Handle) item.station.StationName = data.GetAttributeValue<string>(Som.StationOC.StationName);
                    }
                }
            }
            for (int i = 0; i < manager.UAVObjects.Count; i++)
            {
                if (data.ObjectInstance.Handle == manager.UAVObjects[i].Item2.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {

                        if (data.IsValueUpdated(Som.UAVOC.LeftBattery))
                            manager.UAVObjects[i].Item2.uav.LeftBattery = data.GetAttributeValue<float>(Som.UAVOC.LeftBattery);
                        if (data.IsValueUpdated(Som.UAVOC.Position))
                        {
                            manager.UAVObjects[i].Item2.uav.Position = data.GetAttributeValue<Coordinate>(Som.UAVOC.Position);
                            manager.window.StationOutputBox.Text += "Position: " + manager.UAVObjects[i].Item2.uav.Position.Latitude + "," + manager.UAVObjects[i].Item2.uav.Position.Longitude + Environment.NewLine;
                        }
                        if (data.IsValueUpdated(Som.UAVOC.Radius))
                            manager.UAVObjects[i].Item2.uav.Radius = data.GetAttributeValue<int>(Som.UAVOC.Radius);
                        if (data.IsValueUpdated(Som.UAVOC.Speed))
                            manager.UAVObjects[i].Item2.uav.Speed = data.GetAttributeValue<int>(Som.UAVOC.Speed);
                        if (data.IsValueUpdated(Som.UAVOC.TargetPosition))
                            manager.UAVObjects[i].Item2.uav.TargetPosition = data.GetAttributeValue<Coordinate>(Som.UAVOC.TargetPosition);
                        if (data.IsValueUpdated(Som.UAVOC.UAVName))
                        {
                            manager.UAVObjects[i].Item2.uav.UAVName = data.GetAttributeValue<string>(Som.UAVOC.UAVName);
                            if (manager.UAVObjects[i].Item1 == 0)
                            {
                                Station.AddUAVEntity(manager.window, manager.UAVObjects[i].Item2);
                                manager.UAVObjects[i] = new Tuple<int, CUAVHlaObject>(1, manager.UAVObjects[i].Item2);
                            }
                        }
                        if (data.IsValueUpdated(Som.UAVOC.UAVType))
                            manager.UAVObjects[i].Item2.uav.UAVType = data.GetAttributeValue<string>(Som.UAVOC.UAVType);
                        if (data.IsValueUpdated(Som.UAVOC.VictimList))
                            manager.UAVObjects[i].Item2.uav.VictimList = data.GetAttributeValue<List<string>>(Som.UAVOC.VictimList);
                        if (data.IsValueUpdated(Som.UAVOC.VictimsFound))
                            manager.UAVObjects[i].Item2.uav.VictimsFound = data.GetAttributeValue<bool>(Som.UAVOC.VictimsFound);
                    }
                }
            }
            for (int i = 0; i < manager.VictimObjects.Count; i++)
            {
                if (data.ObjectInstance.Handle == manager.VictimObjects[i].Item2.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.VictimOC.Location.Handle) manager.VictimObjects[i].Item2.victim.Location = data.GetAttributeValue<Coordinate>(Som.VictimOC.Location);
                        else if (pair.Handle == Som.VictimOC.Status.Handle)
                        {
                            manager.VictimObjects[i].Item2.victim.Status = data.GetAttributeValue<VictimStatus>(Som.VictimOC.Status);
                        }
                        else if (pair.Handle == Som.VictimOC.VictimDeviceName.Handle)
                        {
                            manager.VictimObjects[i].Item2.victim.VictimDeviceName = data.GetAttributeValue<string>(Som.VictimOC.VictimDeviceName);
                            if (manager.VictimObjects[i].Item1 == 0)
                            {
                                Station.AddVictimEntity(manager.window, manager.VictimObjects[i].Item2);
                                manager.VictimObjects[i] = new Tuple<int, CVictimHlaObject>(1, manager.VictimObjects[i].Item2);
                            }
                        }
                    }
                }
            }
            #endregion //User Code
        }
        public override void FdAmb_InteractionReceivedHandler(object sender, HlaInteractionEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_InteractionReceivedHandler(sender, data);

            #region User Code
            if (data.Interaction.ClassHandle == Som.StationCommsIC.Handle)
            {
                foreach (var item in data.Interaction.Parameters)
                {
                    if (Som.StationCommsIC.UAVName.Handle == item.Handle)
                    {
                        string msg = data.GetParameterValue<string>(Som.StationCommsIC.UAVName);
                        if (msg.Contains("FOUND"))
                        {
                            string[] a = msg.Split(',');
                            if (!manager.foundVictimNames.Contains(a[1]))
                            {
                                manager.numberOfFoundVictims++;
                                manager.foundVictimNames.Add(a[1]);
                            }
                            
                            SendServedVictims(a[1]);
                        }
                        manager.window.StationOutputBox.Text += "Received interaction: " + msg + Environment.NewLine;

                    }
                }
            }
            if (data.Interaction.ClassHandle == Som.VictimContactIC.Handle)
            {
                foreach (var item in data.Interaction.Parameters)
                {
                    if (Som.VictimContactIC.StatusMessage.Handle == item.Handle)
                    {
                        manager.window.StationOutputBox.Text += "Received interaction: " + data.GetParameterValue<string>(Som.VictimContactIC.StatusMessage) + Environment.NewLine;
                    }
                }
            }
            #endregion //User Code
        }
        public bool SendMessageToStation(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.StationCommsIC, "UAVName");

            interaction.AddParameterValue(Som.StationCommsIC.UAVName, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public bool SendServedVictims(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.StationCommsIC, "ServedVictims");

            interaction.AddParameterValue(Som.StationCommsIC.ServedVictims, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public bool SendMessageToUAV(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.UAVCommsIC, "UAVName");

            interaction.AddParameterValue(Som.UAVCommsIC.UAVName, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public bool SendMessageToVictim(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.VictimContactIC, "StatusMessage");

            interaction.AddParameterValue(Som.VictimContactIC.StatusMessage, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public void UpdateStationName(CStationHlaObject station)
        {
            station.AddAttributeValue(Som.StationOC.StationName, station.station.StationName);
            UpdateAttributeValues(station);
        }
        public void UpdateLocation(CStationHlaObject station)
        {
            station.AddAttributeValue(Som.StationOC.Location, station.station.Location);
            UpdateAttributeValues(station);
        }
        #endregion //Manually Added Code
        #endregion
    }
}
