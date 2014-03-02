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
        Kml kml;
        private LoadMap loadmap;
        public ConvertToKml(LoadMap loadmap)
        {
            this.loadmap = loadmap;
            kml = new Kml();
            ConvertPolyLinesToKml();
        }

        private async void ConvertPolyLinesToKml()
        {
            KmlFile kmlF;
            foreach(YouMapPoint yp in loadmap.Points)
            {
                Point p = new Point();
                Placemark pl = new Placemark();
                p.Coordinate = yp.getLocationAsVector();
                pl.Geometry = p;
                kml.Feature = pl;
                

            }
            foreach(YouMapPolyline yp in loadmap.Polylines)
            {
                LineString ln = new LineString();
                ln.Coordinates = yp.LocationAsCords;
                Placemark pl = new Placemark();
                pl.Geometry = ln;
                kml.Feature = pl;
            }
            
            kmlF = KmlFile.Create(kml, false);
            StorageFolder root = await IOFile.getMyRootfolder();
            StorageFile newFile = await root.CreateFileAsync("Bobby.kml", CreationCollisionOption.ReplaceExisting);


            //Serializer serializer = new Serializer();
            //serializer.Serialize(kmlF.Root);
            

            // using (IRandomAccessStream fileStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
            //{

            //    using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
            //    {
            //        using (DataWriter dataWriter = new DataWriter(outputStream))
            //        {
                        
            //            //dataWriter.W
            //           IBuffer buffer =  dataWriter.DetachBuffer();
            //            await FileIO.WriteBufferAsync(newFile,buffer);
            //            await dataWriter.StoreAsync();
            //            dataWriter.DetachStream();
            //        }

            //        await outputStream.FlushAsync();
            //    }
            //}



            using (IRandomAccessStream fileStream = await newFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                 Stream myStream = fileStream.AsStreamForWrite();
                 kmlF.Save(myStream); 
               
            }


            
            
         }

          

        }
    }

