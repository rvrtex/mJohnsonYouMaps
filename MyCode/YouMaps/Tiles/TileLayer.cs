using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace YouMaps.Tiles
{
    class TileLayer : Panel
    {
        
        private readonly MatrixTransform transform = new MatrixTransform();
        private List<Tile> tiles = new List<Tile>();

        
            
        public void Initialize()
        {
            RenderTransform = transform;
        }


        protected Panel TileContainer
        {
            get{return Parent as Panel;}
        }

        protected void RenderTiles()
        {
            Children.Clear();
            foreach(Tile tile in tiles);
            {
                Children.Add(tile.Image);
            }
        }
    }
    }

