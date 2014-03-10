using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.TappedStates
{
    public interface BaseTappedState
    {
       void Exacute(Object obj, LoadMap loadMap, Location location);
    }
}
