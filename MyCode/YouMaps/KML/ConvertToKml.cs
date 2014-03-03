using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using YouMaps.Points;


namespace YouMaps.KML
{
    class ConvertToKml
    {
        Document doc; 
        Kml kml;
        private LoadMap loadmap;
        public ConvertToKml(LoadMap loadmap)
        {
            this.loadmap = loadmap;
            doc = new Document();
            ConvertFileToKml();
        }

        private async void ConvertFileToKml()
        {
            KmlFile kmlF;
            foreach(YouMapPoint yp in loadmap.Points)
            {
                Point p = new Point();
                Placemark pl = new Placemark();
                p.Coordinate = yp.getLocationAsVector();
                pl.Geometry = p;
                doc.AddFeature(pl);
                

            }
            foreach(YouMapPolyline yp in loadmap.Polylines)
            {
                LineString ln = new LineString();
                ln.Coordinates = yp.LocationAsCords;
                Placemark pl = new Placemark();
                pl.Geometry = ln;
                doc.AddFeature(pl);

               
            }
            kml = new Kml();
            kml.Feature = doc;
            kmlF = KmlFile.Create(kml, false);
            StorageFolder root = await IOFile.getMyRootfolder();
            StorageFile newFile = await root.CreateFileAsync("Bobby.kml", CreationCollisionOption.ReplaceExisting);


            


            using (IRandomAccessStream fileStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                 Stream myStream = fileStream.AsStreamForWrite();
                 kmlF.Save(myStream); 
               
            }

         }

          public LoadMap ConvertKmltoMap()
        {


            Parser parser = new Parser();
             
            return null;
        }

        }
    }

