﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YouMapsPortableClassLibrary
{
    public class Class1
    {
           private async void ConvertPolyLinesToKml(KmlFile passedInKmlFile)
        {
            KmlFile kmlF = passedInKmlFile;
            string bobby = "Bobby";
           
            
            byte[] byteArray = new byte[1000];

            using (MemoryStream sourceStream = File.Open(bobby, FileMode.OpenOrCreate))
            {
                kmlF.Save((Stream)sourceStream);
            }

            //var file = FileIO.
            //IRandomAccessStream stream = await newFile.OpenAsync(FileAccessMode.ReadWrite);

            //var buffer = new Windows.Storage.Streams.Buffer(1024 * 1024 * 10);
            //await FileIO.WriteBufferAsync(newFile, buffer);



            

        }
    }
}
