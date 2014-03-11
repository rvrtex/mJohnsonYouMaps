using MapControl;
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

        public async void ConvertFileToKml()
        {
            KmlFile kmlF;
            foreach(YouMapPoint yp in loadmap.Points)
            {
                Point p = new Point();
                Placemark pl = new Placemark();
                p.Coordinate = yp.getLocationAsVector();
                pl.Geometry = p;
                pl.Name = yp.Name;
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
            foreach(ImagePoint ip in loadmap.Images)
            {
                Point p = new Point();
                Placemark pl = new Placemark();
                Description ds = new Description { Text = ip.WebURL };
                p.Coordinate = ip.getLocationAsVector();
                pl.Geometry = p;
                pl.Description = ds;
                pl.Name = ip.Name;
            }
            if(true)
            {
                
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

          public LoadMap ConvertKmltoMap(Kml kml)
        {

            LoadPolylinesToMap(kml);
             
            return loadmap;
        }

          public void LoadPolylinesToMap(Kml tempKml)
          {
              int locationInLocationsArray = 0;
              bool incrament = false;
              foreach (var placemark in tempKml.Flatten().ToArray())
              {
                 
                  if (placemark is LineString)
                  {
                      
                      LineString placeMark2 = placemark as LineString;
                      CoordinateCollection cord = placeMark2.Coordinates;
                      YouMapPolyline poly = new YouMapPolyline();
                      LocationCollection locationCollection = new LocationCollection();

                      foreach (Vector p in cord.ToList())
                      {
                          if (loadmap == null)
                          {
                              loadmap = new LoadMap(new MapControl.Location { Latitude = p.Latitude, Longitude = p.Longitude });
                              loadmap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection, LocationAsCords = cord });

                          }
                          
                          MapControl.Location location = new MapControl.Location { Latitude = p.Latitude, Longitude = p.Longitude };
                          loadmap.Polylines.ElementAt(locationInLocationsArray).Locations.Add(location);

                      }
                      locationInLocationsArray++;
                      locationCollection = new LocationCollection();
                      loadmap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection, LocationAsCords = cord });

                      
                      Debug.WriteLine(placeMark2.Coordinates);
                  }
                 


              }
          }

        }
    }

