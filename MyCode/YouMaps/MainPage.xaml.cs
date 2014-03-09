

using MapControl;
using SharpKml.Dom;
using SharpKml.Engine;
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
using Windows.Storage.Streams;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YouMaps.KML;
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

        private async void ImportImage()
        { 
            ConvertToKml cKml = new ConvertToKml(null);
            StorageFolder myfolder = await IOFile.getMyRootfolder();
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".kml");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".kmz");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picker1";
            filePicker.CommitButtonText = "Open File to Process";

            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                string extension = Path.GetExtension(file.Path);
                if (extension.Equals(".kml"))
                {
                    KmlFile kmlFile;
                    LoadMap newMap = new LoadMap(null);

                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        Stream myStream = fileStream.AsStreamForWrite();
                        kmlFile = KmlFile.Load(myStream);

                        Kml kml = kmlFile.Root as Kml;
                        if (kml != null)
                        {
                            newMap = cKml.ConvertKmltoMap(kml);
                        }
                    }



                    (App.Current as App).SavedMapLoading = newMap;
                    this.Frame.Navigate(typeof(MapPage));


                }
            }
            //string folderName = DropDownOfFolders.SelectedValue.ToString();
            //StorageFolder selectedFolder = await myfolder.GetFolderAsync(folderName);
            //FileOpenPicker openfile = new FileOpenPicker();
            //openfile.ViewMode = PickerViewMode.List;
            //openfile.SuggestedStartLocation = PickerLocationId.Downloads;
          

            //StorageFile originalFile = await openfile.PickSingleFileAsync();
            //if(originalFile != null)
            //{
            //    //StorageFile filePathToCopy = await originalFile.CopyAsync(selectedFolder, originalFile.Name, NameCollisionOption.GenerateUniqueName);
            //   // Messages.Text = "File Import Done";
            //}
                


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
        MapControl.Location customLocation = new MapControl.Location();
       

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

        private async void CurrentLocationClicked(object sender, TappedRoutedEventArgs e)
        {
            await GetCurrentGPSLocation();

            this.Frame.Navigate(typeof(MapPage));
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
            FrameworkElement tempFrame = sender as FrameworkElement;
            FlyoutBase.ShowAttachedFlyout(tempFrame);
            
            
              
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
            this.Frame.Navigate(typeof(ManageSymbols));
        }
        IFrontPageTile convertLongLat;
        private void ConverLongLatLoaded(object sender, RoutedEventArgs e)
        {
            convertLongLat = new ManageSymbolTile { Image = "/Assets/ConvertLongLatImage.jpg", Title = "Convert Longitude and Latitude", Subtitle = "Convert to degrees or to decimal" };
            ConvertLongLatTiles.Add(convertLongLat);
            var gridView = (GridView)sender;
            gridView.ItemsSource = ConvertLongLatTiles;
        }

        private void ConvertLongLat(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            Grid tempGrid = tempButton.Parent as Grid;
            TextBox temLong;
            TextBox tempLat;
            try
            {
                temLong = VisualTreeHelper.GetChild(tempGrid, 0) as TextBox;
                tempLat = VisualTreeHelper.GetChild(tempGrid, 1) as TextBox;
                double latitude = Double.Parse(tempLat.Text);
                double longitude = Double.Parse(temLong.Text);
                double[] firstPartsLat = new double[3];
                double[] firstPartsLong = new double[3];



                firstPartsLat[0] = Math.Truncate(latitude);
                double tempNumLat = latitude - Math.Truncate(latitude);
                firstPartsLat[1] = Math.Truncate(tempNumLat * 60);
                tempNumLat = (tempNumLat * 60) - firstPartsLat[1];
                firstPartsLat[2] = tempNumLat * 60;
                firstPartsLat[2] = Math.Round(firstPartsLat[2], 5);

                firstPartsLong[0] = Math.Truncate(latitude);
                double tempNumLong = latitude - Math.Truncate(latitude);
                firstPartsLong[1] = Math.Truncate(tempNumLong * 60);
                tempNumLong = (tempNumLong * 60) - firstPartsLat[1];
                firstPartsLong[2] = tempNumLong * 60;
                firstPartsLong[2] = Math.Round(firstPartsLat[2], 5);

                string preConversion = "Pre-conversion: \nLatitude: " + latitude + "\nLongitude: " + longitude+"\n";
                string postconversion = "Post Conversion: \nLatitude: " + firstPartsLat[0] + " degrees " + firstPartsLat[1] + "\' " + firstPartsLat[2] + "\"" + "\nLongitude: " + firstPartsLong[0] + " degrees " + firstPartsLong[1] + "\' " + firstPartsLong[2] + "\"";
                //MessageBox.Visibility = Visibility.Visible;
                tempGrid.Children.Clear();
                TextBlock conversionBox = new TextBlock { Text = preConversion + postconversion, FontSize = 25, };
                
                tempGrid.Children.Add(conversionBox);
                
                Button thanksButton = new Button();
                thanksButton.Click += ThanksClick;
                thanksButton.Content = "Thanks!";
                tempGrid.Children.Add(thanksButton);

                 
            }
            catch
            {

            }




        }

        private void ThanksClick(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            Grid tempGrid = tempButton.Parent as Grid;
            FlyoutPresenter flyout = tempGrid.Parent as FlyoutPresenter;
            Popup temp = flyout.Parent as Popup;
            temp.IsOpen = false;
        }
        private void ConvertLongLatClicked(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement tempFrame = sender as FrameworkElement;
            FlyoutBase.ShowAttachedFlyout(tempFrame);

        }

        private void ImportFiles(object sender, RoutedEventArgs e)
        {
            IFrontPageTile importFileTile = new ManageSymbolTile { Image = "/Assets/importFile.jpg", Title = "Import Files", Subtitle = "Import jpg, JPEG, or KML files" };
            ImportFileTiles.Add(importFileTile);
            var gridView = (GridView)sender;
            gridView.ItemsSource = ImportFileTiles;
        }
        private async void ImportFileClicked(object sender, TappedRoutedEventArgs e)
        {
            ImportImage();
        }

        private void GoToCustomLocation(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            Grid tempGrid = tempButton.Parent as Grid;
            TextBox temLong;
            TextBox tempLat;
            try
            {
                temLong = VisualTreeHelper.GetChild(tempGrid,0) as TextBox;
                tempLat = VisualTreeHelper.GetChild(tempGrid, 1) as TextBox;

                customLocation.Latitude = Double.Parse(tempLat.Text);
                
                customLocation.Longitude = Double.Parse(temLong.Text);
            }
            catch
            {
                customLocation.Latitude = 0;
                customLocation.Longitude = 0;
            }
            (App.Current as App).CurrentLocation = customLocation;


            this.Frame.Navigate(typeof(MapPage));
        }

       

        
        

       

        

   
      

      

        

       

    }

}

