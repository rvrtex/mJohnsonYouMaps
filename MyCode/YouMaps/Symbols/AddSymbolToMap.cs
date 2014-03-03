using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.Symbols
{
    class AddSymbolToMap
    {
        LoadMap loadMap;
        Location pointerPressedLocation;

        public AddSymbolToMap(LoadMap loadMap, Location pointerPressedLocation)
        {
            this.loadMap = loadMap;
            this.pointerPressedLocation = pointerPressedLocation;
        }



    }
}
