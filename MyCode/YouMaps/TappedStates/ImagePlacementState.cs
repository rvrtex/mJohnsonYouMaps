using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouMaps.Points;

namespace YouMaps.TappedStates
{
    class ImagePlacementState : BaseTappedState
    {
        public void Exacute(Object obj, LoadMap loadMap, Location location)
        {
            ImagePoint image = (ImagePoint)obj;
            image.Location = location;
            loadMap.Images.Add(image);
            (App.Current as App).CurrentTappedState = new NormalState();
            
        }
    }
}
