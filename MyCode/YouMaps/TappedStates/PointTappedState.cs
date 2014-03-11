using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouMaps.Points;

namespace YouMaps.TappedStates
{
    class PointTappedState :BaseTappedState
    {
        public void Exacute(object obj, LoadMap loadMap, MapControl.Location location)
        {
            YouMapPoint point = (YouMapPoint)obj;
            point.Location = location;
            if(string.IsNullOrWhiteSpace(point.Name))
            {
                double Lat = Math.Round(location.Latitude,4);
                double Long = Math.Round(location.Longitude,4);
                string LatLong = "Lat: "+Lat+" Long: "+Long;
                point.Name = LatLong;
            }
            loadMap.Points.Add(point);

            (App.Current as App).CurrentTappedState = new NormalState();
        }
    }
}
