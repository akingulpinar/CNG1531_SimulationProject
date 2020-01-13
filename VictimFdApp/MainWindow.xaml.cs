using Racon;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using UAVProject.Som;

namespace UAVProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CVictimFdApp federate;
        public CVictimHlaObject victim;
        public BindingList<CUAVHlaObject> UAVObjects;
        public BindingList<CStationHlaObject> StationObjects;
        public BindingList<CVictimHlaObject> VictimObjects;
        public bool IsRadioOn = false;

        public MainWindow()
        {
            InitializeComponent();

            UAVObjects = new BindingList<CUAVHlaObject>();
            StationObjects = new BindingList<CStationHlaObject>();
            VictimObjects = new BindingList<CVictimHlaObject>();

            federate = new CVictimFdApp(this);
            victim = new CVictimHlaObject(federate.Som.VictimOC);
            victim.victim = new Models.Victim();
            VictimObjects.Add(victim);

            // Initialize the federation execution
            federate.FederationExecution.FederateName = victim.Name;
            federate.FederationExecution.Name = "UAVDisasterFederationExecution";
            federate.FederationExecution.FederateType = "VictimFederateType";
            federate.FederationExecution.ConnectionSettings = "rti://10.144.31.177";
            federate.FederationExecution.FDD = @".\UAVFOM.xml";

            federate.LogLevel = Racon.LogLevel.ALL;
            federate.FederateStateChanged += Federate_FederateStateChanged;
            federate.InteractionReceived += federate.FdAmb_InteractionReceivedHandler;
            federate.StatusMessageChanged += new EventHandler(CUAVFd_StatusMessageChanged);
        }

        public void BeginSimulation()
        {
            federate.Connect(callbackModel: Racon.CallbackModel.EVOKED, "");
            federate.CreateFederationExecution(federate.FederationExecution.Name, federate.FederationExecution.FDD, "");
            federate.JoinFederationExecution(federate.FederationExecution.FederateName, federate.FederationExecution.FederateType, federate.FederationExecution.Name);

            federate.DeclareCapability();
        }

        private void MainSimulationLoop(object sender, EventArgs e)
        {
            if (federate.FederateState.HasFlag(Racon.FederateStates.JOINED))
            {
                federate.Run();
            }
        }

        private void SendSOSMessage()
        {
            federate.SendMessage(VictimObjects[0].victim.Location.Latitude.ToString()+","+ VictimObjects[0].victim.Location.Longitude.ToString()+","+VictimObjects[0].victim.VictimDeviceName);
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

        private void CUAVFd_StatusMessageChanged(object sender, EventArgs e)
        {
            VictimOutputBox.Text += federate.StatusMessage + System.Environment.NewLine;
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            Coordinate coord = new Coordinate
            {
                Latitude = Convert.ToInt32(xValueBox.Text),
                Longitude = Convert.ToInt32(yValueBox.Text)
            };
            VictimObjects[0].victim.Location = coord;
            BeginSimulation();

            CompositionTarget.Rendering += new EventHandler(MainSimulationLoop);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            federate.DeleteObjectInstance(VictimObjects[0], "");
            federate.ResignFederationExecution(action: ResignAction.DELETE_OBJECTS);
            federate.DestroyFederationExecution("UAVDisasterFederationExecution");
            federate.Disconnect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendSOSMessage();
        }
    }
}
