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
    class PointManager 
    {
        public ObservableCollection<BasePoint> Points { get; set; }

        public PointManager()
        {
            Points = new ObservableCollection<BasePoint>();
        }
        public void CreatePoint(string name, Location location)
        {
            
            Point point = new Point();
            point.Location = location;
            point.Name = name;
            Points.Add(point);

            
        }
    }
}
