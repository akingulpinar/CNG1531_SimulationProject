using Racon.RtiLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UAVProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CSimulationManager manager;

        private int _uavAmount;
        public int uavAmount
        {
            get { return _uavAmount; }
            set
            {
                _uavAmount = value;
                if (_uavAmount >= 3)
                {
                    manager.federate.RegisterFederationSynchronizationPoint("uavReady", "UAV's are ready");
                }
            }
        }

        public int victimAmount { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            manager = new CSimulationManager(this);
            manager.federate.StatusMessageChanged += new EventHandler(CUAVFd_StatusMessageChanged);

            manager.BeginSimulation();
            CompositionTarget.Rendering += new EventHandler(MainSimulationLoop);
        }
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in manager.UAVObjects)
            {
                for (int i = 0; i < canv.Children.Count; i++)
                {
                    if (canv.Children[i].Uid.Equals(item.Item2.uav.UAVName))
                    {
                        DoubleAnimation anim = new DoubleAnimation(0, canv.ActualWidth - item.Item2.uav.Radius, 
                            new Duration(TimeSpan.Parse("0:0:" + item.Item2.uav.Speed)));

                        TranslateTransform trans = new TranslateTransform();
                        canv.RenderTransform = trans;

                        anim.RepeatBehavior = RepeatBehavior.Forever;
                        anim.AutoReverse = true;
                        int x = i;

                        anim.CurrentTimeInvalidated += (ss, ee) =>
                        {
                            List<HlaAttribute> attributes = new List<HlaAttribute>();
                            attributes.Add(manager.federate.Som.UAVOC.Position);
                            manager.federate.RequestAttributeValueUpdate(item.Item2, attributes, 
                                (item.Item2.uav.Position.Latitude + 10) + "," + (item.Item2.uav.Position.Longitude));
                        };
                        canv.Children[i].BeginAnimation(Canvas.LeftProperty, anim);
                    }
                }
            }

            //for (int i = 0; i < manager.UAVObjects.Count; i++)
            //{
            //    if (canv.Children[i].Uid.Equals(manager.UAVObjects[i].Item2.uav.UAVName))
            //    {
            //        DoubleAnimation anim = new DoubleAnimation(0, canv.ActualWidth - manager.UAVObjects[i].Item2.uav.Radius, new Duration(TimeSpan.Parse("0:0:" + manager.UAVObjects[i].Item2.uav.Speed)));

            //        TranslateTransform trans = new TranslateTransform();
            //        canv.RenderTransform = trans;

            //        //anim.RepeatBehavior = RepeatBehavior.Forever;
            //        //anim.AutoReverse = true;
            //        int x = i;

            //        anim.CurrentTimeInvalidated += (ss, ee) =>
            //        {                      
            //            List<HlaAttribute> attributes = new List<HlaAttribute>();
            //            attributes.Add(manager.federate.Som.UAVOC.Position);
            //            manager.federate.RequestAttributeValueUpdate(manager.UAVObjects[x].Item2, attributes, (manager.UAVObjects[x].Item2.uav.Position.Latitude + 10) + "," + (manager.UAVObjects[x].Item2.uav.Position.Longitude));
            //        };
            //        canv.Children[i].BeginAnimation(Canvas.LeftProperty, anim);                   
            //    }
            //}
        }
        private void MainSimulationLoop(object sender, EventArgs e)
        {
            if (manager.federate.FederateState.HasFlag(Racon.FederateStates.JOINED))
            {
                manager.federate.Run();
            }
        }
        private void CUAVFd_StatusMessageChanged(object sender, EventArgs e)
        {
            StationOutputBox.Text += manager.federate.StatusMessage + System.Environment.NewLine;
        }
        private void SendInteractionButton_Click(object sender, RoutedEventArgs e)
        {
            manager.federate.SendMessageToStation("TESTMESSAGE TESTMESSAGE TESTMESSAGE");
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            manager.UAVObjects[0].Item2.uav.LeftBattery = 100.0f;
            manager.federate.UpdateAttributeValues(manager.UAVObjects[0].Item2, "");
            manager.federate.RequestAttributeValueUpdate(manager.UAVObjects[0].Item2, "");
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ySize.Content = "y:" + this.Width;
            xSize.Content = "x:" + this.Height;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (manager.numberOfFoundVictims >= victimAmount)
            {
                manager.federate.RegisterFederationSynchronizationPoint("allReported", "All victims are reported");
            }
        }
    }
}
