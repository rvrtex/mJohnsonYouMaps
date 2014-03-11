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

        private Location location = new Location();
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
             
            }
        }

        private Vector locationAsVector = new Vector();

        public Vector getLocationAsVector()
        {
            locationAsVector.Latitude = location.Latitude;
            locationAsVector.Longitude = location.Longitude;
            return locationAsVector;
        }
        public  string WebURL { get; set; }

       
    }
}
