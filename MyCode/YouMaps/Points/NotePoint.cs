using MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouMaps.Points
{
    public class NotePoint : BasePoint
    {
        public string Content { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
    }
}
