using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YouMaps
{
    class LoadProperties
    {
        public LoadProperties()
        {
            //loadFolders();

        }
        private IReadOnlyList<StorageFolder> folders;
        public async Task<IReadOnlyList<StorageFolder>> loadFolders()
        {
            int count;
            StorageFolder rootFolder = KnownFolders.PicturesLibrary;
            StorageFolder myfolder = await rootFolder.GetFolderAsync("YouMapsImages");
            
            folders = await myfolder.GetFoldersAsync();
             count = folders.Count();
           if(count == 0)
           {
               await myfolder.CreateFolderAsync("default", CreationCollisionOption.FailIfExists);
               await myfolder.GetFoldersAsync();
           }

           return folders;
            
        }

        public IReadOnlyList<StorageFolder> getFolders()
        {
            if (folders != null)
            {
                return folders;
            }
            else
                return null;
        }
    }
}
