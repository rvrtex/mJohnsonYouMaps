using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps
{
    class LoadMap
    {
        private Location mapCenter;
        public ObservableCollection<YouMaps.Points.BasePoint> Points { get; set; }
        public Location MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;
                //OnPropertyChanged("MapCenter");
            }
        }

        public LoadMap(Location location)
        {

            MapCenter = location;
            //PointManager pm = new PointManager();
            //Location loc = new Location();
            //loc.Latitude = 53.4;
            //loc.Longitude = 8.3;
            //pm.CreatePoint("My Point", loc);
            //Points = pm.Points;
        }

        
    }
}
