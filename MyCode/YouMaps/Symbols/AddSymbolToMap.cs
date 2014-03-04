using MapControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace YouMaps.Symbols
{
    class AddSymbolToMap
    {
        
        Point pointerPressedLocation;

        public AddSymbolToMap(Point pointerPressedLocation)
        {            
            this.pointerPressedLocation = pointerPressedLocation;
        }

        

        public async Task<List<PointCollection>> ChangePointToPlaceSymbol()
        {
            YouMapsSymbol currentSymbol = (App.Current as App).CurrentSymbolToBePlaced;
            List<PointCollection> pointsToChange = currentSymbol.SymbolPoints.ToList<PointCollection>();
            double centerX = currentSymbol.HighX - currentSymbol.LowX;
            double centerY = currentSymbol.HighY - currentSymbol.LowY;
            Point centerPoint = new Point{X = centerX, Y = centerY};
            Point topLeft = new Point{X = pointerPressedLocation.X-centerPoint.X, Y = pointerPressedLocation.Y - centerPoint.Y};

            List<PointCollection> changedPoints = new List<PointCollection>();
            foreach(PointCollection pc in pointsToChange)
            {
                PointCollection newPointsCollection = new PointCollection();
                foreach(Point p in pc)
                {
                    Point newPoint = new Point{X = topLeft.X+p.X,Y=topLeft.Y+p.Y};
                    Debug.WriteLine("PointerPressed: " + pointerPressedLocation);
                    Debug.WriteLine("X: " + newPoint.X + " Y: " + newPoint.Y);
                    newPointsCollection.Add(newPoint);
                }
                changedPoints.Add(newPointsCollection);
            }
            return changedPoints;
        }
        
    }
}
