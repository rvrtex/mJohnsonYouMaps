using Bing.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit;


namespace YouMaps.MyImages
{
    class ImageDownloader
    {
        Map myMap = new Map();
        Geolocator geo = null;
        string apiKey = "AnglDgpN7Ckz6x82rUAuewNPmykjr5Th4-XqL6jDsvrnfXaMaNzukCY84-xPUQop";
        string imagerySet = MapType.Aerial.ToString();

        public async Task<WriteableBitmap> RequestImage()
        {
            
            if (geo == null)
            {
                geo = new Geolocator();
            }
            Geoposition pos = await geo.GetGeopositionAsync();
            Geopoint point = pos.Coordinate.Point;

            myMap.Center = new Location(point.Position.Latitude, point.Position.Longitude);
            myMap.ZoomLevel = 12;
            myMap.Width = 100;
            this.myMap.Height = 100;
            string url = String.Format("http://dev.virtualearth.net/REST/v1/Imagery/Map/{0}/{1},{2}/{3}" +
                                  "?mapSize={4},{5}&key={6}",
                               imagerySet,
                               this.myMap.Center.Latitude,
                               this.myMap.Center.Longitude,
                               Math.Floor(this.myMap.ZoomLevel),
                               this.myMap.Width,
                               this.myMap.Height,
                               apiKey);


            var backgroundDownloader = new BackgroundDownloader();
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("tempmapimage.jpg",
                CreationCollisionOption.ReplaceExisting);
            var downloadOperation = backgroundDownloader.CreateDownload(new Uri(url), file);

            await downloadOperation.StartAsync();
            return await BitmapFactory.New(1, 1).FromStream(await file.OpenReadAsync(),
                Windows.Graphics.Imaging.BitmapPixelFormat.Unknown);
        }
    }
}
