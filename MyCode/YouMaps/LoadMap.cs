using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps
{
    class LoadMap
    {
        private Location mapCenter;
        public Location MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;
                //OnPropertyChanged("MapCenter");
            }
        }

        public LoadMap()
        {
            MapCenter = new Location(53.5, 8.2);
        }

    }
}
