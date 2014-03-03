using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using YouMaps.Symbols;
using YouMaps.UserControls;

namespace YouMaps
{
    class IOFile
    {
        public static async Task<StorageFolder> getMyRootfolder()
        {
            StorageFolder rootFolder = KnownFolders.PicturesLibrary;
            StorageFolder myfolder = await rootFolder.GetFolderAsync("YouMapsImages");
            return myfolder;
        }

        public static async Task<StorageFolder> getMySymbolsfolder()
        {
            bool folderNotFound = false;
            StorageFolder rootFolder = KnownFolders.PicturesLibrary;
            StorageFolder myfolder = await rootFolder.GetFolderAsync("YouMapsImages");
            try
            {
                myfolder = await myfolder.GetFolderAsync("Symbols");
            }
            catch (FileNotFoundException e)
            {
                folderNotFound = true;

            }
            if (folderNotFound)
            {
                await myfolder.CreateFolderAsync("Symbols");
                myfolder = await myfolder.GetFolderAsync("Symbols");
            }
            return myfolder;
        }

        public static async Task<List<YouMapsSymbol>> LoadAllSymbols()
        {
            
            
            List<YouMapsSymbol> allSymbols = new List<YouMapsSymbol>();
            StorageFolder folder = await IOFile.getMySymbolsfolder();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile f in files)
            {
                try
                {
                    IInputStream sessionInputStream = await f.OpenReadAsync();
                    var sessionSerializer = new DataContractSerializer(typeof(YouMapsSymbol));
                    var loadedSymbol = sessionSerializer.ReadObject(sessionInputStream.AsStreamForRead());

                    YouMapsSymbol fullyLoadedSymbol = (YouMapsSymbol)loadedSymbol;
                    allSymbols.Add(fullyLoadedSymbol);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("A file was the wrong type");
                    Debug.WriteLine(ex.Message);
                }
            }
            return allSymbols;
        }

        private static List<YouMapsSymbol> symbolsOnList = new List<YouMapsSymbol>();
        public async static Task<List<YouMapsSymbol>> getSucOnList()
        {
            symbolsOnList.Clear();
            List<YouMapsSymbol> allSymbols = await IOFile.LoadAllSymbols();

            foreach (YouMapsSymbol s in allSymbols)
            {
                if (s.OnSymbolList)
                {
                    symbolsOnList.Add(s);
                }
            }
            return symbolsOnList;
        }
    }
}
