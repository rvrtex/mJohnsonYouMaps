using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //ConvertFileToKml();
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

          public async Task<LoadMap> ConvertKmltoMap(StorageFile file)
        {
            KmlFile kmlFile; 
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                Stream myStream = fileStream.AsStreamForWrite();
                kmlFile = KmlFile.Load(myStream);

                Kml kml = kmlFile.Root as Kml;
                if(kml != null)
                {
                    foreach(var placemark in kml.Flatten().OfType<LineString>())
                    {
                        CoordinateCollection cord = placemark.Coordinates;
                        Debug.WriteLine(placemark.Coordinates);
                        
                    }
                }
            }

            Parser parser = new Parser();
             
            return null;
        }

        }
    }

