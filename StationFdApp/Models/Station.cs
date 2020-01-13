using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UAVProject.Som;

namespace UAVProject.Models
{
    public class Station
    {
        public Coordinate Location;
        public string StationName;

        public Station()
        {
            Location = new Coordinate
            {
                Longitude = 0,
                Latitude = 0
            };
            StationName = "StationFederate" + Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
        public static void AddUAVEntity(MainWindow w, CUAVHlaObject uav)
        {
            int posRad = 0;
            for (int t = 0; t < w.manager.UAVObjects.Count - 1; t++)
            {
                posRad += w.manager.UAVObjects[t].Item2.uav.Radius;
            }

            Canvas c = new Canvas
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Height = uav.uav.Radius
            };

            Ellipse e = new Ellipse
            {
                Opacity = 0.2,
                Fill = System.Windows.Media.Brushes.LightGreen,
                Height = uav.uav.Radius,
                Stroke = System.Windows.Media.Brushes.Green,
                Width = uav.uav.Radius,
                RenderTransformOrigin = new System.Windows.Point(0, 0)
            };

            Image i = new Image
            {
                Height = 34,
                Width = 34,
                RenderTransformOrigin = new System.Windows.Point(0, 0)
            };

            Canvas.SetLeft(i, (uav.uav.Radius / 2) - 17);
            Canvas.SetTop(c, posRad);
            Canvas.SetTop(i, (uav.uav.Radius / 2) - 17);

            i.Source = new BitmapImage(new Uri("img/uav.png", UriKind.Relative));
            c.Children.Add(e);
            c.Children.Add(i);
            c.Uid = uav.uav.UAVName.ToString();
            w.canv.Children.Add(c);
            w.uavAmount += 1;
            w.uavCount.Content = w.uavAmount.ToString();
        }
        public static void AddVictimEntity(MainWindow w, CVictimHlaObject victim)
        {
            Canvas c = new Canvas
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 34
            };
            Image i = new Image
            {
                Height = 34,
                Width = 34,
                RenderTransformOrigin = new System.Windows.Point(0, 0)
            };
            Canvas.SetLeft(c, victim.victim.Location.Latitude);
            Canvas.SetTop(c, victim.victim.Location.Longitude);

            i.Source = new BitmapImage(new Uri("img/victim.png", UriKind.Relative));
            c.Children.Add(i);
            c.Uid = victim.victim.VictimDeviceName.ToString();
            w.canv.Children.Add(c);
            w.victimAmount += 1;
            w.victimCount.Content = w.victimAmount.ToString();
        }
        public static void RemoveEntity(MainWindow w, UAV uav)
        {
            for (int i = 0; i < w.canv.Children.Count; i++)
            {
                if (w.canv.Children[i].Uid.Equals(uav.UAVName))
                {
                    w.canv.Children.Remove(w.canv.Children[i]);
                }
            }
            if (Convert.ToInt32(w.uavCount.Content) - 1 < 0)
            {
                w.uavCount.Content = "0";
            }
            else
            {
                w.uavCount.Content = (Convert.ToInt32(w.uavCount.Content) - 1).ToString();
            }

        }
        public static void RemoveEntity(MainWindow w, Victim victim)
        {
            for (int i = 0; i < w.canv.Children.Count; i++)
            {
                if (w.canv.Children[i].Uid.Equals(victim.VictimDeviceName))
                {
                    w.canv.Children.Remove(w.canv.Children[i]);
                }
            }
            if (Convert.ToInt32(w.uavCount.Content) - 1 < 0)
            {
                w.victimCount.Content = "0";
            }
            else
            {
                w.victimCount.Content = (Convert.ToInt32(w.victimCount.Content) - 1).ToString();
            }
        }
    }
}