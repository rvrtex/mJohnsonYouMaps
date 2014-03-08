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
    public class LoadMap : BasePoint
    {
        private Location mapCenter;
        public ObservableCollection<YouMapPoint> Points { get; set; }
        public ObservableCollection<YouMapPolyline> Polylines { get; set; }

        public ObservableCollection<ImagePoint> Images { get; set; }
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

           // MapCenter = location;
            MapCenter = location;
            Points = new ObservableCollection<YouMapPoint>();
            Polylines = new ObservableCollection<YouMapPolyline>();

        }

        
    }
}
