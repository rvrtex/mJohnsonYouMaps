using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

    }
}
