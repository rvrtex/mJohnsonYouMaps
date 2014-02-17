using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouMaps.Points;

namespace YouMaps
{
    public class PointManager 
    {
        public ObservableCollection<YouMapPoint> Points { get; set; }

        public PointManager()
        {
            Points = new ObservableCollection<YouMapPoint>();
        }
        public void CreatePoint(string name, Location location)
        {
            
            YouMapPoint point = new YouMapPoint();
            point.Location = location;
            point.Name = name;
            Points.Add(point);

            
        }
    }
}
