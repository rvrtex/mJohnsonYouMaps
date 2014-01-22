using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace YouMaps.Tiles
{
    class TileLoader
    {
        public const string defaultCasheName = "YouMapsTileCashe";
     //   public static string defaultCasheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "YouMaps");
        public static string defaultCasheDirectory = "here for now";

        private ConcurrentQueue<Tile> tilesInQue = new ConcurrentQueue<Tile>();
        private int numOfThreads;

        protected void StartGetTiles(TileLayer tileLayer, IEnumerable<Tile> tiles)
        {
            
        }

    }
}
