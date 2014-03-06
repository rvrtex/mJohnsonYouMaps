

using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YouMaps.MainPafeTiles;
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
        private ObservableCollection<IFrontPageTile> CurrentLocationTiles = new ObservableCollection<IFrontPageTile>();
        private ObservableCollection<IFrontPageTile> CustomLocationTiles = new ObservableCollection<IFrontPageTile>();
        private ObservableCollection<IFrontPageTile> ManageSymbolsTiles = new ObservableCollection<IFrontPageTile>();
        private ObservableCollection<IFrontPageTile> ConvertLongLatTiles = new ObservableCollection<IFrontPageTile>();
        private ObservableCollection<IFrontPageTile> ImportFileTiles = new ObservableCollection<IFrontPageTile>();





        
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
               // DropDownOfFolders.ItemsSource = folderName;
                
                
            }
            
        }

        private async void ImportImage(object sender, RoutedEventArgs e)
        {
            StorageFolder myfolder = await IOFile.getMyRootfolder();
            //string folderName = DropDownOfFolders.SelectedValue.ToString();
            //StorageFolder selectedFolder = await myfolder.GetFolderAsync(folderName);
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
                //StorageFile filePathToCopy = await originalFile.CopyAsync(selectedFolder, originalFile.Name, NameCollisionOption.GenerateUniqueName);
               // Messages.Text = "File Import Done";
            }
                


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private async void CreateFolder(object sender, RoutedEventArgs e)
        {
            string newFoldPromt = "New folder name here.";
            string noFolderEnteredMessage = "You must enter a new folder name";
            //if(!NewFolderNameTextBox.Text.Equals(newFoldPromt))
            //{
            //    StorageFolder myfolder = await IOFile.getMyRootfolder();
            //    await myfolder.CreateFolderAsync(NewFolderNameTextBox.Text);
            //    Messages.Text = "FolderCreated";
            //    NewFolderNameTextBox.Text = newFoldPromt;
            //    loadComboBox();
            //}
            //else
            //{
            //    Messages.Text = noFolderEnteredMessage;
            //}
        }

        
       
        private async void GetCurrentLocation(object sender, RoutedEventArgs e)
        {

            await GetCurrentGPSLocation();
            
            this.Frame.Navigate(typeof(MapPage));
          
        }
        private async Task GetCurrentGPSLocation()
        {
            Geolocator geo = null;
            if (geo == null)
            {
                geo = new Geolocator();

            }
            
           
            Geoposition currentLocation = await geo.GetGeopositionAsync();
            customLocation.Longitude = currentLocation.Coordinate.Longitude;
            customLocation.Latitude = currentLocation.Coordinate.Latitude;
            (App.Current as App).CurrentLocation = customLocation;
          


        }
        Location customLocation = new Location();
        private void GetImputedLocation(object sender, RoutedEventArgs e)
        {
            

            try
            {
                //customLocation.Latitude = Double.Parse(Latitude.Text);
                //customLocation.Longitude = Double.Parse(Longitude.Text);
            }
            catch
            {
                customLocation.Latitude = 0;
                customLocation.Longitude = 0;
            }
            (App.Current as App).CurrentLocation = customLocation;
            

            this.Frame.Navigate(typeof(MapPage));
            



            
            //await id.RequestImage();
        }

        private async void TakePicture(object sender, RoutedEventArgs e)
        {
            
            CameraCaptureUI cameraUi = new CameraCaptureUI();

            cameraUi.PhotoSettings.AllowCropping = false;
            cameraUi.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.MediumXga;

            StorageFolder folder = await IOFile.getMyRootfolder();
            StorageFile photo = await cameraUi.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo != null)
            {
                BitmapImage bitmapCamera = new BitmapImage();

                using (var streamCamera = await photo.OpenAsync(FileAccessMode.Read))
                {
                    bitmapCamera.SetSource(streamCamera);


                    int width = bitmapCamera.PixelWidth;
                    int height = bitmapCamera.PixelHeight;

                    WriteableBitmap wBitmap = new WriteableBitmap(width, height);
                    await photo.CopyAsync(folder, photo.DisplayName, NameCollisionOption.ReplaceExisting);
                    using (var stream = await photo.OpenAsync(FileAccessMode.Read))
                    {
                        wBitmap.SetSource(stream);
                    }
                }
            }
        }

        private void Convert(object sender, RoutedEventArgs e)
        {
            
            

            
            //double latitude = Double.Parse(Latitude.Text);
            //double longitude = Double.Parse(Longitude.Text);

            //double[] firstParts = new double[3];

            //firstParts[0] = Math.Truncate(latitude);
            //double tempNum = latitude-Math.Truncate(latitude);
            //firstParts[1] = Math.Truncate(tempNum * 60);
            //tempNum = (tempNum * 60) - firstParts[1];
            //firstParts[2] = tempNum * 60;
            //firstParts[2] = Math.Round(firstParts[2], 5);

            //Converted.Text = ""+firstParts[0]+" degrees "+firstParts[1]+"\' "+firstParts[2]+"\"";
       }

        private async void LoadBoxes(object sender, RoutedEventArgs e)
        {
            
           
           
            
            
        }


        private async void LoadCurrentLocation(object sender, RoutedEventArgs e)
        {
            Geolocator geo = null;
            if (geo == null)
            {
                geo = new Geolocator();

            }
            
            Geoposition currentLocation = await geo.GetGeopositionAsync();
            customLocation.Longitude = currentLocation.Coordinate.Longitude;
            customLocation.Latitude = currentLocation.Coordinate.Latitude;
            var gridView = (GridView)sender;
            string currentLocationLongLat = "" + Math.Round(customLocation.Latitude, 3) + " " + Math.Round(customLocation.Longitude, 3);
            IFrontPageTile currentLocationTile = new CurrentLocationTile { Image = "/Assets/currentLocationPin.jpg", Title = "Current Location", Subtitle = currentLocationLongLat };
            CurrentLocationTiles.Add(currentLocationTile);
            gridView.ItemsSource = CurrentLocationTiles;
        }

        private void CurrentLocationClicked(object sender, TappedRoutedEventArgs e)
        {

        }

        private void LoadCustomLocation(object sender, RoutedEventArgs e)
        {
            IFrontPageTile customLocationTile = new CustomLocationTile { Image = "/Assets/customLocationWatch.jpg", Title = "Custom Location", Subtitle = "Click to enter a location of your choice" };
            CustomLocationTiles.Add(customLocationTile);
            var gridView = (GridView)sender;
            gridView.ItemsSource = CustomLocationTiles;
            
        }

        private void CustomLocationClicked(object sender, TappedRoutedEventArgs e)
        {

        }
        private void LoadManageSymbol(object sender, RoutedEventArgs e)
        {

            IFrontPageTile manageSymbols = new ManageSymbolTile { Image = "/Assets/symbols.jpg", Title = "Manage Symbols", Subtitle = "Create, Edit, Delete, and all to sidebar" };
            ManageSymbolsTiles.Add(manageSymbols);
            var gridView = (GridView)sender;
            gridView.ItemsSource = ManageSymbolsTiles;
        }
       

        private void ManageSymbolsClicked(object sender, TappedRoutedEventArgs e)
        {

        }
        private void ConverLongLatLoaded(object sender, RoutedEventArgs e)
        {
            IFrontPageTile convertLongLat = new ManageSymbolTile { Image = "/Assets/ConvertLongLatImage.jpg", Title = "Convert Longitude and Latitude", Subtitle = "Convert to degrees or to decimal" };
            ConvertLongLatTiles.Add(convertLongLat);
            var gridView = (GridView)sender;
            gridView.ItemsSource = ConvertLongLatTiles;
        }
        
        private void ConvertLongLatClicked(object sender, TappedRoutedEventArgs e)
        {


        }

        private void ImportFiles(object sender, RoutedEventArgs e)
        {
            IFrontPageTile importFileTile = new ManageSymbolTile { Image = "/Assets/importFile.jpg", Title = "Import Files", Subtitle = "Import jpg, JPEG, or KML files" };
            ImportFileTiles.Add(importFileTile);
            var gridView = (GridView)sender;
            gridView.ItemsSource = ImportFileTiles;
        }
        private void ImportFileClicked(object sender, TappedRoutedEventArgs e)
        {
            
        }

        

       

        

   
      

      

        

       

    }

}

