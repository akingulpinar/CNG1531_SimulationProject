using Racon;
using System;
using System.ComponentModel;
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
        public CUAVFdApp federate;
        public CUAVHlaObject uav;
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

            federate = new CUAVFdApp(this);
            uav = new CUAVHlaObject(federate.Som.UAVOC);
            uav.uav = new Models.UAV();
            UAVObjects.Add(uav);

            // Initialize the federation execution
            federate.FederationExecution.FederateName = uav.Name;
            federate.FederationExecution.Name = "UAVDisasterFederationExecution";
            federate.FederationExecution.FederateType = "UAVFederateType";
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
            UAVOutputBox.Text += federate.StatusMessage + System.Environment.NewLine;
        }

        private void SendInteractionButton_Click(object sender, RoutedEventArgs e)
        {
            federate.SendMessage("TESTMESSAGE TESTMESSAGE TESTMESSAGE");
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            uav.uav.Radius = Convert.ToInt32(radBox.Text);
            uav.uav.Speed = Convert.ToInt32(speedBox.Text);

            BeginSimulation();

            CompositionTarget.Rendering += new EventHandler(MainSimulationLoop);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            federate.DeleteObjectInstance(UAVObjects[0], UAVObjects[0].Handle);
            federate.ResignFederationExecution(action: ResignAction.DELETE_OBJECTS);
            federate.DestroyFederationExecution("UAVDisasterFederationExecution");
            federate.Disconnect();
        }
    }
}
