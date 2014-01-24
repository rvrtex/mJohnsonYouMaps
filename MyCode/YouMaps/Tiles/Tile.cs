using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace YouMaps.Tiles
{
    class Tile
    {
        public readonly int zoomLevel;
        public readonly int xAxis;
        public readonly int yAxis;
  //      public readonly Image image = new Image();
        public bool HasImageSource { get; private set; }


        public Tile(int xAxis, int yAxis,int zoomLevel)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.zoomLevel = zoomLevel;
            
        }

        //public ImageSource getImageSource()
        //{
        //    return image.Source;
        //}

        public void setImageSource(ImageSource image)
        {
  //          this.image.Source = image;
            HasImageSource = true;
        }

        public int Xindex
        {
            get
            {
                var numberOfTiles = 1 << zoomLevel;
                return ((xAxis % numberOfTiles) + numberOfTiles) % numberOfTiles;
                 
            }
        }







    }
}
