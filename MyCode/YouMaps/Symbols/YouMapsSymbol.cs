using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace YouMaps.Symbols
{

    [DataContract]
    [KnownType(typeof(PointCollection))]
    [KnownType(typeof(Point))]
    public class YouMapsSymbol
    {
        [DataMember]
        public ObservableCollection<PointCollection> SymbolPoints { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double HighX { get; set; }
        [DataMember]
        public double HighY { get; set; }
    }
}
