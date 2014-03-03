using YouMaps.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Windows.UI;
using System.Runtime.Serialization;
using Windows.Storage;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using YouMaps.Symbols;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using YouMaps.UserControls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace YouMaps
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    [DataContract]
    public sealed partial class ManageSymbols : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public ManageSymbols()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            CanvasPolylines = new ObservableCollection<Polyline>();
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            CanvaseGrid.Visibility = Visibility.Collapsed;
            SaveLoadGrid.Visibility = Visibility.Collapsed;
            EditGrid.Visibility = Visibility.Collapsed;
            DeleteGrid.Visibility = Visibility.Collapsed;
            DrawingCanvas.PointerPressed += drawingPointerIsPressed;
            DrawingCanvas.PointerReleased += drawingPointerReleased;
            DrawingCanvas.PointerMoved += drawingPointerHasMoved;

           // DrawingCanvas.PointerEntered += drawingPointerEnteredCanvas;
            //CreateSymbolPopup.PointerExited += drawingPointerExitedObject;
        }

        private void drawingPointerEnteredCanvas(object sender, PointerRoutedEventArgs e)
        {
            
            
        }
        [DataMember]
        Polyline polyLine;
        private bool mousePressed;
        private double highY = 300;
        private double highX = 300;
        public ObservableCollection<Polyline> CanvasPolylines { get; set; }
        private void drawingPointerHasMoved(object sender, PointerRoutedEventArgs e)
        {
            if(mousePressed)
            {
                PointerPoint pp = e.GetCurrentPoint(DrawingCanvas);
                polyLine.Points.Add(new Point {X = pp.Position.X,Y=pp.Position.Y });
                if(pp.Position.X < highX)
                {
                    highX = pp.Position.X;

                }
                if(pp.Position.Y < highY)
                {
                    highY = pp.Position.Y;
                }
                
            }
            
            Debug.WriteLine(DrawingCanvas.Children.Count);
            
        }

        private void drawingPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if(mousePressed)
            {
                
                mousePressed = false;
            }
        }

        [DataMember]
        ObservableCollection<PointCollection> pointsForSaveing = new ObservableCollection<PointCollection>();
        private void drawingPointerIsPressed(object sender, PointerRoutedEventArgs e)
        {
            double thickness = 10;
             
            polyLine = new Polyline { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = thickness };
            PointCollection pointCollection = new PointCollection();
            polyLine.Points = pointCollection;
            pointsForSaveing.Add(pointCollection);
            DrawingCanvas.Children.Add(polyLine);
            mousePressed = true;
           
        }


        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public void AddSymbol(object sender, RoutedEventArgs e)
        {
            CanvaseGrid.Visibility = Visibility.Visible;
            SaveLoadGrid.Visibility = Visibility.Visible;
            InfoGrid.Visibility = Visibility.Collapsed;
            SymbolGrid.Visibility = Visibility.Collapsed;
            EditYouMapSymbolsStackPanel.Visibility = Visibility.Collapsed;
            EditCustomSymbol.Visibility = Visibility.Collapsed;

            
            
        }

        List<SymbolUserControl> sucs = new List<SymbolUserControl>();
        private async void EditSymbol(object sender, RoutedEventArgs e)
        {
            List<YouMapsSymbol> allSymbols = await IOFile.LoadAllSymbols();
            EditYouMapSymbolsStackPanel.Visibility = Visibility.Visible;
            SymbolGrid.Visibility = Visibility.Collapsed;
            EditYouMapSymbolsStackPanel.Children.Clear();
            sucs.Clear();
            foreach(YouMapsSymbol yms in allSymbols)
            {
                DrawingCanvas.Children.Clear();
                DeleteGrid.Visibility = Visibility.Collapsed;
                EditGrid.Visibility = Visibility.Visible;
                SymbolUserControl suc = new SymbolUserControl(yms);
                suc.CheckBoxEverything.Visibility = Visibility.Collapsed;
                sucs.Add(suc);
                suc.CheckBoxEverything.Visibility = Visibility.Collapsed;
                EditCustomSymbol.Visibility = Visibility.Visible;
                EditYouMapSymbolsStackPanel.Children.Add(suc);
               
            }


        }

        

        private async void DeleteSymbol(object sender, RoutedEventArgs e)
        {
            List<YouMapsSymbol> allSymbols = await IOFile.LoadAllSymbols();
            EditYouMapSymbolsStackPanel.Visibility = Visibility.Visible;
            SymbolGrid.Visibility = Visibility.Collapsed;
            EditYouMapSymbolsStackPanel.Children.Clear();
            sucs.Clear();
            EditYouMapSymbolsStackPanel.Children.Clear();
            foreach (YouMapsSymbol yms in allSymbols)
            {

                SymbolUserControl suc = AddSymbolsToEditDeleteGrid(yms);
                DeleteGrid.Visibility = Visibility.Visible;
                
            }
        }

        private async void AddSymbolEasyAccess(object sender, RoutedEventArgs e)
        {
            SymbolGrid.Visibility = Visibility.Collapsed;
            List<YouMapsSymbol> allSymbols = await IOFile.LoadAllSymbols();
            sucs.Clear();
            foreach(YouMapsSymbol yms in allSymbols)
            {
                SymbolUserControl suc = AddSymbolsToEditDeleteGrid(yms);
                suc.CheckBoxEverything.IsChecked = yms.OnSymbolList;                
            }
        }

        private SymbolUserControl AddSymbolsToEditDeleteGrid(YouMapsSymbol yms)
        {
            SymbolUserControl suc = new SymbolUserControl(yms);
            suc.EditRadioButton.Visibility = Visibility.Collapsed;
            sucs.Add(suc);
            suc.CheckBoxEverything.Visibility = Visibility.Visible;
            EditYouMapSymbolsStackPanel.Children.Add(suc);
            EditGrid.Visibility = Visibility.Collapsed;
            return suc;
        }
        string messageOne = "Enter Symbol Name Here";
        string messageTwo = "You Must Enter a Name";
        YouMapsSymbol currentSymbol = new YouMapsSymbol();
        private async void SaveSymbol(object sender, RoutedEventArgs e)
        {
            bool saveReady = true;
            currentSymbol.SymbolPoints = pointsForSaveing;
            currentSymbol.HighX = highX;
            currentSymbol.HighY = highY;
            currentSymbol.Name = SymbolName.Text;
            
            if(currentSymbol.Name.Equals(messageOne) || currentSymbol.Name.Equals(messageTwo))
            {
                saveReady = false;
                SymbolName.Text = (SymbolName.Text.Equals(messageOne)) ? messageTwo : messageOne;
            }
            if (saveReady)
            {
                MemoryStream symbolData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(currentSymbol.GetType());
                serializer.WriteObject(symbolData, currentSymbol);

                StorageFolder folder = await IOFile.getMySymbolsfolder();
                StorageFile file = await folder.CreateFileAsync(SymbolName.Text, CreationCollisionOption.ReplaceExisting);

                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    symbolData.Seek(0, SeekOrigin.Begin);
                    await symbolData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();

                }
                DrawingCanvas.Children.Clear();
                CanvaseGrid.Visibility = Visibility.Collapsed;
                SaveLoadGrid.Visibility = Visibility.Collapsed;
                InfoGrid.Visibility = Visibility.Visible;
                SymbolGrid.Visibility = Visibility.Visible;
                (App.Current as App).CurrentSymbol = null;
                MessageBox.Text = "Your symbol has been saved";
            }
        }
        
      
        private void EnteredTextBox(object sender, PointerRoutedEventArgs e)
        {
            SymbolName.Text = " ";
        }

        private void ModifySymbol(object sender, RoutedEventArgs e)
        {
            foreach(SymbolUserControl s in sucs)
            {
                bool checker = (bool)s.EditRadioButton.IsChecked;
                if(checker)
                {
                    (App.Current as App).CurrentSymbol = s.YouMapsSymbol;

                    foreach (PointCollection p in s.YouMapsSymbol.SymbolPoints)
                    {
                        double thickness = 10;

                        polyLine = new Polyline { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = thickness };
                        polyLine.Points = p;
                        SymbolName.Text = s.YouMapsSymbol.Name;
                        DrawingCanvas.Children.Add(polyLine);
                        AddSymbol(sender,e);

                    }
                }
            }
        }

        private async void DeleteSymbolButton(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = await IOFile.getMySymbolsfolder();
            
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            List<string> namesToDelete = new List<string>();
            
            foreach(SymbolUserControl s in sucs)
            {
              
                bool checker = (bool)s.CheckBoxEverything.IsChecked;
                if(checker)
                {
                    namesToDelete.Add(s.YouMapsSymbol.Name);
                }
            }

            foreach (StorageFile f in files)
            {
                if (namesToDelete.Contains(f.DisplayName))
                {
                    await f.DeleteAsync();
                }
            }

            DeleteSymbol(sender, e);
        }


        
        private void SaveSymbolPanelButton(object sender, RoutedEventArgs e)
        {

            List<YouMapsSymbol> listToSave = new List<YouMapsSymbol>();
            foreach(SymbolUserControl s in sucs)
            {
                bool checker = (bool)s.CheckBoxEverything.IsChecked;
                if (checker)
                {
                    s.YouMapsSymbol.OnSymbolList = true;

                }
                else
                {
                    s.YouMapsSymbol.OnSymbolList = false;
                }
                listToSave.Add(s.YouMapsSymbol);
            }
            UpdateSaveSymbolFiles(listToSave);
        }

        private async void UpdateSaveSymbolFiles(List<YouMapsSymbol> listOfSymbols)
        {
            foreach (YouMapsSymbol s in listOfSymbols)
            {
                MemoryStream symbolData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(s.GetType());
                serializer.WriteObject(symbolData, s);

                StorageFolder folder = await IOFile.getMySymbolsfolder();
                StorageFile file = await folder.CreateFileAsync(s.Name, CreationCollisionOption.ReplaceExisting);

                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    symbolData.Seek(0, SeekOrigin.Begin);
                    await symbolData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    

                }
            //    DrawingCanvas.Children.Clear();
            //    CanvaseGrid.Visibility = Visibility.Collapsed;
            //    SaveLoadGrid.Visibility = Visibility.Collapsed;
            //    InfoGrid.Visibility = Visibility.Visible;
            //    SymbolGrid.Visibility = Visibility.Visible;
            //    (App.Current as App).CurrentSymbol = null;
            //    MessageBox.Text = "Your symbol has been saved";
            }
        }
    }
}
