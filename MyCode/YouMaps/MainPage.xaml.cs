

using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YouMaps.MyImages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YouMaps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        LoadProperties lp; 
        public MainPage()
        {
            lp = new LoadProperties();
            this.InitializeComponent();
            loadComboBox();

            

        }

        private async void loadComboBox()
        {

            IReadOnlyList<StorageFolder> folders = await lp.loadFolders();
            

            if (folders != null)
            {

                ObservableCollection<string> folderName = new ObservableCollection<string>();
                foreach(StorageFolder s in folders)
                {
                    folderName.Add(s.Name);
                }
                DropDownOfFolders.ItemsSource = folderName;
                
                
            }
            
        }

        private async void ImportImage(object sender, RoutedEventArgs e)
        {
            StorageFolder myfolder = await getMyRootfolder();
            string folderName = DropDownOfFolders.SelectedValue.ToString();
            StorageFolder selectedFolder = await myfolder.GetFolderAsync(folderName);
            FileOpenPicker openfile = new FileOpenPicker();
            openfile.ViewMode = PickerViewMode.List;
            openfile.SuggestedStartLocation = PickerLocationId.Downloads;
            openfile.FileTypeFilter.Add(".kml");
            openfile.FileTypeFilter.Add(".jpeg");
            openfile.FileTypeFilter.Add(".kmz");
            openfile.FileTypeFilter.Add(".jpg");

            StorageFile originalFile = await openfile.PickSingleFileAsync();
            if(originalFile != null)
            {
                StorageFile filePathToCopy = await originalFile.CopyAsync(selectedFolder, originalFile.Name, NameCollisionOption.GenerateUniqueName);
                Messages.Text = "File Import Done";
            }
                


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void CreateFolder(object sender, RoutedEventArgs e)
        {
            string newFoldPromt = "New folder name here.";
            string noFolderEnteredMessage = "You must enter a new folder name";
            if(!NewFolderNameTextBox.Text.Equals(newFoldPromt))
            {
                StorageFolder myfolder = await getMyRootfolder();
                await myfolder.CreateFolderAsync(NewFolderNameTextBox.Text);
                Messages.Text = "FolderCreated";
                NewFolderNameTextBox.Text = newFoldPromt;
                loadComboBox();
            }
            else
            {
                Messages.Text = noFolderEnteredMessage;
            }
        }

        private static async Task<StorageFolder> getMyRootfolder()
        {
            StorageFolder rootFolder = KnownFolders.PicturesLibrary;
            StorageFolder myfolder = await rootFolder.GetFolderAsync("YouMapsImages");
            return myfolder;
        }

        private void GetCurrentLocation(object sender, RoutedEventArgs e)
        {
            
            this.Frame.Navigate(typeof(MapPage));
          
        }

        private async void GetImputedLocation(object sender, RoutedEventArgs e)
        {
            //ImageDownloader id = new ImageDownloader();

            Location customLocation = new Location();
            customLocation.Latitude = Double.Parse(Latitude.Text);
            customLocation.Longitude = Double.Parse(Longitude.Text);
            this.Frame.Navigate(typeof(MapPage),customLocation);
            



            
            //await id.RequestImage();
        }
    }
}
