using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapControl;
using SharpKml.Base;

namespace YouMaps.Points
{
    public class ImagePoint : BasePoint
    {
        public string Name { get; set; }

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
        public  string WebURL { get; set; }

       
    }
}
