using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouMaps.Points;

namespace YouMaps
{
    class LoadMap : BasePoint
    {
        private Location mapCenter;
        public ObservableCollection<YouMaps.Points.YouMapPoint> Points { get; set; }
        public ObservableCollection<YouMaps.Points.YouMapPolyline> PolyLines { get; set; }
        public Location MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;
                OnPropertyChanged("MapCenter");
            }
        }
        

        public LoadMap(Location location)
        {

            MapCenter = location;
            Points = new ObservableCollection<YouMapPoint>();
            PolyLines = new ObservableCollection<YouMapPolyline>();
           // PointManager pm = new PointManager();
            //Location loc = new Location();
            //loc.Latitude = 53.4;
            //loc.Longitude = 8.3;
            //Points.Add(new YouMapPoint
            //{
            //    Name = "This is a point",
            //    Location = new Location(location.Latitude,location.Longitude)

            //});
            //pm.CreatePoint("My Point", location);
            //Points = pm.Points;
        }

        
    }
}
