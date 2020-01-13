// **************************************************************************************************
//		CUAVFdApp
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
// Application
using UAVProject.Som;

namespace UAVProject
{
    public partial class CUAVFdApp : Racon.CGenericFederate
    {
        #region Manually Added Code

        // Local Data
        MainWindow window;
        private object thisLock = new object();

        #region Constructor
        public CUAVFdApp(MainWindow w) : this()
        {
            window = w;
        }
        #endregion //Constructor

        #region Federation Management Callbacks
        public override void FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationConfirmedHandler(sender, data);

            #region User Code
            window.UAVOutputBox.Text += "Confirmed sync point!" + Environment.NewLine;
            #endregion //User Code
        }
        public override void FdAmb_OnSynchronizationPointRegistrationFailedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationFailedHandler(sender, data);

            #region User Code
            window.UAVOutputBox.Text += "Failed sync point!" + Environment.NewLine;
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
                window.UAVOutputBox.Text += "Checking battery!!!" + Environment.NewLine;
                window.UAVOutputBox.Text += window.uav.uav.UAVName + Environment.NewLine;
                window.UAVOutputBox.Text += window.uav.uav.LeftBattery + Environment.NewLine;
                if (window.uav.uav.LeftBattery > 75)
                {
                    window.UAVOutputBox.Text += window.uav.uav.UAVName + " is ready." + Environment.NewLine;
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            else if (data.Label.Equals("allReported"))
            {
                window.UAVOutputBox.Text += "All victims are reported to the station!!!" + Environment.NewLine;
                check = true;
            }
            window.federate.SynchronizationPointAchieved(data.Label, check);
            #endregion //User Code
        }
        public override void FdAmb_FederationSynchronized(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_FederationSynchronized(sender, data);

            #region User Code
            window.UAVOutputBox.Text += "Federate synchronized!" + Environment.NewLine;
            #endregion //User Code
        }
        #endregion
        #region Declaration Management Callbacks
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            if (data.ObjectClassHandle == Som.UAVOC.Handle)
                RegisterHlaObject(window.UAVObjects[0]);
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
            window.IsRadioOn = false;
            #endregion //User Code
        }
        public override void FdAmb_TurnInteractionsOnAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_TurnInteractionsOnAdvisedHandler(sender, data);

            #region User Code
            window.IsRadioOn = true;
            #endregion //User Code
        }
        #endregion
        #region Object Management Callbacks
        public override void FdAmb_InteractionReceivedHandler(object sender, HlaInteractionEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_InteractionReceivedHandler(sender, data);

            string message;
            #region User Code
            if (data.Interaction.ClassHandle == Som.UAVCommsIC.Handle)
            {
                foreach (var item in data.Interaction.Parameters)
                {
                    if (Som.UAVCommsIC.UAVName.Handle == item.Handle)
                    {
                        window.UAVOutputBox.Text += "Received interaction " + data.GetParameterValue<string>(Som.UAVCommsIC.UAVName);
                    }
                }
            }
            if (data.Interaction.ClassHandle == Som.VictimContactIC.Handle)
            {
                foreach (var item in data.Interaction.Parameters)
                {
                    if (Som.VictimContactIC.StatusMessage.Handle == item.Handle)
                    {
                        message = data.GetParameterValue<string>(Som.VictimContactIC.StatusMessage);
                        window.UAVOutputBox.Text += "Received interaction " + message;

                        string[] a = message.Split(',');
                        Coordinate coord = new Coordinate
                        {
                            Latitude = Convert.ToInt32(a[0]),
                            Longitude = Convert.ToInt32(a[1])
                        };
                        string vName = a[2];

                        double la = window.UAVObjects[0].uav.Position.Latitude;
                        double lo = window.UAVObjects[0].uav.Position.Longitude;
                        int r = window.UAVObjects[0].uav.Radius;
                        if ((la + r >= coord.Latitude && la - r <= coord.Latitude))
                        {
                            window.federate.SendMessage("FOUND VICTIM,"+vName);
                        }
                    }
                }
            }
            #endregion //User Code
        }
        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            if (data.ClassHandle == Som.UAVOC.Handle)
            {
                CUAVHlaObject uav = new CUAVHlaObject(data.ObjectInstance);
                window.UAVObjects.Add(uav);

                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.UAVOC.UAVName);
                attributes.Add(Som.UAVOC.LeftBattery);
                attributes.Add(Som.UAVOC.Position);
                attributes.Add(Som.UAVOC.Radius);
                attributes.Add(Som.UAVOC.Speed);
                attributes.Add(Som.UAVOC.TargetPosition);
                attributes.Add(Som.UAVOC.UAVType);
                attributes.Add(Som.UAVOC.VictimList);
                attributes.Add(Som.UAVOC.VictimsFound);
                RequestAttributeValueUpdate(uav, attributes, "");
            }
            #endregion //User Code
        }
        public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectAttributesReflectedHandler(sender, data);

            #region User Code
            foreach (var item in window.UAVObjects)
            {
                if (data.ObjectInstance.Handle == item.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.UAVOC.LeftBattery.Handle) item.uav.LeftBattery = pair.GetValue<float>();
                        else if (pair.Handle == Som.UAVOC.Position.Handle) item.uav.Position = pair.GetValue<Coordinate>();
                        else if (pair.Handle == Som.UAVOC.Radius.Handle) item.uav.Radius = pair.GetValue<int>();
                        else if (pair.Handle == Som.UAVOC.Speed.Handle) item.uav.Speed = pair.GetValue<int>();
                        else if (pair.Handle == Som.UAVOC.TargetPosition.Handle) item.uav.TargetPosition = pair.GetValue<Coordinate>();
                        else if (pair.Handle == Som.UAVOC.UAVName.Handle) item.uav.UAVName = pair.GetValue<string>();
                        else if (pair.Handle == Som.UAVOC.UAVType.Handle) item.uav.UAVType = pair.GetValue<string>();
                        else if (pair.Handle == Som.UAVOC.VictimList.Handle) item.uav.VictimList = pair.GetValue<List<String>>();
                        else if (pair.Handle == Som.UAVOC.VictimsFound.Handle) item.uav.VictimsFound = pair.GetValue<bool>();
                    }
                }
            }
            #endregion //User Code
        }
        public override void FdAmb_AttributeValueUpdateRequestedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_AttributeValueUpdateRequestedHandler(sender, data);

            #region User Code
            if (data.ObjectInstance.Handle == window.UAVObjects[0].Handle)
            {
                foreach (var item in data.ObjectInstance.Attributes)
                {
                    if (item.Handle == Som.UAVOC.LeftBattery.Handle)
                    {
                        UpdateLeftBattery(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.Position.Handle)
                    {
                        var a = data.Tag.GetData<string>();
                        if (!a.Equals(""))
                        {
                            Coordinate c = new Coordinate
                            {
                                Latitude = Convert.ToDouble(a.Split(',')[0]),
                                Longitude = Convert.ToDouble(a.Split(',')[1])
                            };
                            window.UAVObjects[0].uav.Position = c;
                        }
                        UpdatePosition(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.Radius.Handle)
                    {
                        UpdateRadius(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.Speed.Handle)
                    {
                        UpdateSpeed(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.TargetPosition.Handle)
                    {
                        UpdateTargetPosition(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.UAVName.Handle)
                    {
                        UpdateUAVName(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.UAVType.Handle)
                    {
                        UpdateUAVType(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.VictimList.Handle)
                    {
                        UpdateVictimList(window.UAVObjects[0]);
                    }
                    else if (item.Handle == Som.UAVOC.VictimsFound.Handle)
                    {
                        UpdateVictimsFound(window.UAVObjects[0]);
                    }
                    window.UAVOutputBox.Text += "Update successful, the object:" + item.Name;
                }
            }
            #endregion //User Code
        }
        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            object[] snap;
            if (data.ClassHandle == Som.UAVOC.Handle)
            {
                lock (thisLock)
                {
                    snap = window.UAVObjects.ToArray();
                }
                foreach (CUAVHlaObject uav in snap)
                {
                    if (data.ObjectInstance.Handle == uav.Handle)// Find the Object
                    {
                        window.UAVObjects.Remove(uav);
                        window.UAVOutputBox.Text += ($"UAV: {uav.uav.UAVName} left. Number of UAVs Now: {window.UAVObjects.Count}" + Environment.NewLine);
                    }
                }
            }
            #endregion //User Code
        }
        public bool SendMessage(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.StationCommsIC, "UAVName");

            interaction.AddParameterValue(Som.StationCommsIC.UAVName, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public void UpdateLeftBattery(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.LeftBattery, uav.uav.LeftBattery);
            UpdateAttributeValues(uav, "");
        }
        public void UpdatePosition(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.Position, uav.uav.Position);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateRadius(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.Radius, uav.uav.Radius);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateSpeed(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.Speed, uav.uav.Speed);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateTargetPosition(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.TargetPosition, uav.uav.TargetPosition);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateUAVName(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.UAVName, uav.uav.UAVName);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateUAVType(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.UAVType, uav.uav.UAVType);
            UpdateAttributeValues(uav, "");
        }
        public void UpdateVictimList(CUAVHlaObject uav)
        {
            //uav.AddAttributeValue(Som.UAVOC.VictimList, uav.uav.VictimList);
            //UpdateAttributeValues(uav, "");
        }
        public void UpdateVictimsFound(CUAVHlaObject uav)
        {
            uav.AddAttributeValue(Som.UAVOC.VictimsFound, uav.uav.VictimsFound);
            UpdateAttributeValues(uav, "");
        }
        #endregion
        #endregion //Manually Added Code
    }
}
