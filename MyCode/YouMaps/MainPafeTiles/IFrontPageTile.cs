using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.MainPafeTiles
{
    interface IFrontPageTile
    {
       
        string Image { get; set; }
        string Title { get; set; }
        string Subtitle { get; set; }

        void Execute();
    }
}
