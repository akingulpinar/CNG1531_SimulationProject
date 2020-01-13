// **************************************************************************************************
//		CVictimFdApp
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
// Racon
using Racon.RtiLayer;
using System;
using System.Collections.Generic; // for List
using System.Linq;
// Application
using UAVProject.Som;

namespace UAVProject
{
    public partial class CVictimFdApp : Racon.CGenericFederate
    {
        #region Manually Added Code

        // Local Data
        MainWindow window;
        private object thisLock = new object();

        #region Constructor
        public CVictimFdApp(MainWindow w) : this()
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
            window.VictimOutputBox.Text += "Confirmed sync point!" + Environment.NewLine;
            #endregion //User Code
        }
        public override void FdAmb_OnSynchronizationPointRegistrationFailedHandler(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_OnSynchronizationPointRegistrationFailedHandler(sender, data);

            #region User Code
            window.VictimOutputBox.Text += "Failed sync point!" + Environment.NewLine;
            #endregion //User Code
        }
        public override void FdAmb_SynchronizationPointAnnounced(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_SynchronizationPointAnnounced(sender, data);

            #region User Code
            if (data.Label.Equals("allReported"))
            {
                window.VictimOutputBox.Text += "All victims are reported to the station!!!" + Environment.NewLine;
                window.federate.SynchronizationPointAchieved(data.Label, true);
            }
            #endregion //User Code
        }
        public override void FdAmb_FederationSynchronized(object sender, HlaFederationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_FederationSynchronized(sender, data);

            #region User Code
            window.VictimOutputBox.Text += "Federate synchronized!" + Environment.NewLine;
            #endregion //User Code
        }
        #endregion 
        #region Declaration Management Callbacks
        public override void FdAmb_StartRegistrationForObjectClassAdvisedHandler(object sender, HlaDeclarationManagementEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_StartRegistrationForObjectClassAdvisedHandler(sender, data);

            #region User Code
            if (data.ObjectClassHandle == Som.VictimOC.Handle)
                RegisterHlaObject(window.VictimObjects[0]);
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
        public override void FdAmb_ObjectDiscoveredHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectDiscoveredHandler(sender, data);

            #region User Code
            if (data.ClassHandle == Som.VictimOC.Handle)
            {
                CVictimHlaObject victim = new CVictimHlaObject(data.ObjectInstance);
                window.VictimObjects.Add(victim);

                List<HlaAttribute> attributes = new List<HlaAttribute>();
                attributes.Add(Som.VictimOC.Location);
                attributes.Add(Som.VictimOC.Status);
                attributes.Add(Som.VictimOC.VictimDeviceName);

                RequestAttributeValueUpdate(victim, attributes,"");
            }
            #endregion //User Code
        }
        public override void FdAmb_ObjectRemovedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectRemovedHandler(sender, data);

            #region User Code
            object[] snap;
            if (data.ClassHandle == Som.VictimOC.Handle)
            {
                lock (thisLock)
                {
                    snap = window.VictimObjects.ToArray();
                }
                foreach (CVictimHlaObject victim in snap)
                {
                    if (data.ObjectInstance.Handle == victim.Handle)// Find the Object
                    {
                        window.VictimObjects.Remove(victim);
                        window.VictimOutputBox.Text += ($"Victim: {victim.victim.VictimDeviceName} left. Number of victims Now: {window.VictimObjects.Count}" + Environment.NewLine);
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
            if (data.ObjectInstance.Handle == window.VictimObjects[0].Handle)
            {
                foreach (var item in data.ObjectInstance.Attributes)
                {
                    if (item.Handle == Som.VictimOC.VictimDeviceName.Handle)
                    {
                        UpdateVictimDeviceName(window.VictimObjects[0]);
                    }
                    else if (item.Handle == Som.VictimOC.Location.Handle)
                    {
                        UpdateLocation(window.VictimObjects[0]);
                    }
                    else if (item.Handle == Som.VictimOC.Status.Handle)
                    {
                        UpdateStatus(window.VictimObjects[0]);
                    }

                    window.VictimOutputBox.Text += "Update successful, the object:" + item.Value;
                }
            }
            #endregion
        }
        public override void FdAmb_ObjectAttributesReflectedHandler(object sender, HlaObjectEventArgs data)
        {
            // Call the base class handler
            base.FdAmb_ObjectAttributesReflectedHandler(sender, data);

            #region User Code
            foreach (var item in window.VictimObjects)
            {
                if (data.ObjectInstance.Handle == item.Handle)
                {
                    foreach (var pair in data.ObjectInstance.Attributes)
                    {
                        if (pair.Handle == Som.VictimOC.Location.Handle) item.victim.Location = pair.GetValue<Coordinate>();
                        else if (pair.Handle == Som.VictimOC.Status.Handle) item.victim.Status = pair.GetValue<VictimStatus>();
                        else if (pair.Handle == Som.VictimOC.VictimDeviceName.Handle) item.victim.VictimDeviceName = pair.GetValue<string>();
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
            if (data.Interaction.ClassHandle == Som.VictimContactIC.Handle)
            {
                foreach (var item in data.Interaction.Parameters)
                {
                    if (Som.VictimContactIC.StatusMessage.Handle == item.Handle)
                    {
                        window.VictimOutputBox.Text += "Received interaction " + data.GetParameterValue<string>(Som.VictimContactIC.StatusMessage);
                    }
                }
            }
            #endregion //User Code
        }
        public bool SendMessage(string txt)
        {
            HlaInteraction interaction = new Racon.RtiLayer.HlaInteraction(Som.VictimContactIC, "StatusMessage");

            interaction.AddParameterValue(Som.VictimContactIC.StatusMessage, txt); // String

            return (SendInteraction(interaction, ""));
        }
        public void UpdateVictimDeviceName(CVictimHlaObject victim)
        {
            victim.AddAttributeValue(Som.VictimOC.VictimDeviceName, victim.victim.VictimDeviceName);
            UpdateAttributeValues(victim,"");
        }
        public void UpdateLocation(CVictimHlaObject victim)
        {
            victim.AddAttributeValue(Som.VictimOC.Location, victim.victim.Location);
            UpdateAttributeValues(victim,"");
        }
        public void UpdateStatus(CVictimHlaObject victim)
        {
            victim.AddAttributeValue(Som.VictimOC.Status, Convert.ToInt32(victim.victim.Status));
            UpdateAttributeValues(victim,"");
        }
        #endregion
        #endregion //Manually Added Code
    }
}