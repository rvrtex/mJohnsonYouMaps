using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace YouMaps.Tiles
{
    class TileLayer : Panel
    {
        
        private readonly MatrixTransform transform = new MatrixTransform();
        private List<Tile> tiles = new List<Tile>();
        private TRectangle grid;
        private int zoomLevel;

        public static TileLayer Default
        {
            get
            {
                return new TileLayer
                {
                    SourceName = "OpenStreetMap",
                    Description = "© {y} OpenStreetMap Contributors, CC-BY-SA",
                    TileSource = new TileSource("http://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png")
                };
            }
        }
        public string SourceName { get; set; }
        public TileSource TileSource { get; set; }


        public string Description
        {
            get { return description; }
            set { description = value.Replace("{y}", DateTime.Now.Year.ToString()); }
        }
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
            foreach(var tile in tiles)
            {
                
                Children.Add(tile.image);
            }
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var tile in tiles)
            {
                tile.image.Measure(availableSize);
            }

            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach(var tile in tiles)
            {
                var tileSoze = (double)(256 << zoomLevel - tile.zoomLevel);
                tile.image.Width = tileSoze;
                tile.image.Height = tileSoze;
                tile.image.Arrange(new Rect(tileSoze * tile.xAxis - 256, tileSoze * tile.yAxis - 256 * grid.Y, tileSoze, tileSoze));

            }
            return finalSize;
        }
    }
    }

