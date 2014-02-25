using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace YouMaps.Points
{
    class YouMapPolyline
    {
        public LocationCollection Locations { get; set; }
        public Brush ColorOfLine { get; set; }



        public static object Location { get; set; }
    }
}
