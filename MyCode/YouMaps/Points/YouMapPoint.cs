using MapControl;
using SharpKml.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.Points
{
    public class YouMapPoint : BasePoint
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                locationAsVector.Latitude = location.Latitude;
                locationAsVector.Longitude = location.Longitude;
                OnPropertyChanged("Location");
            }
        }

        private Vector locationAsVector = new Vector();

        public Vector getLocationAsVector()
        {
            return locationAsVector;
        }
    }
}
