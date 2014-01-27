using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace YouMaps
{
    class LongLatTransform
    {
        private static readonly double maxLatitude = Math.Atan(Math.Sinh(Math.PI)) / Math.PI * 180d;

        public override double MaxLatitude
        {
            get { return maxLatitude; }
        }

        public override double RelativeScale(Location location)
        {
            if (location.Latitude <= -90d)
            {
                return double.NegativeInfinity;
            }

            if (location.Latitude >= 90d)
            {
                return double.PositiveInfinity;
            }

            return 1d / Math.Cos(location.Latitude * Math.PI / 180d);
        }

        public override Point Transform(Location location)
        {
            double latitude;

            if (location.Latitude <= -90d)
            {
                latitude = double.NegativeInfinity;
            }
            else if (location.Latitude >= 90d)
            {
                latitude = double.PositiveInfinity;
            }
            else
            {
                latitude = location.Latitude * Math.PI / 180d;
                latitude = Math.Log(Math.Tan(latitude) + 1d / Math.Cos(latitude)) / Math.PI * 180d;
            }

            return new Point(location.Longitude, latitude);
        }

        public override Location Transform(Point point)
        {
            var latitude = Math.Atan(Math.Sinh(point.Y * Math.PI / 180d)) / Math.PI * 180d;

            return new Location(latitude, point.X);
        }
    }
    }
}
