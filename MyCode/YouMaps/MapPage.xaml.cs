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

using Windows.Devices.Geolocation;

using MapControl;
using System.ComponentModel;
using Windows.UI.Input;
using Windows.UI;
using System.Diagnostics;
using YouMaps.UserControls;
using YouMaps.KML;
using YouMaps.Symbols;
using System.Threading.Tasks;
using YouMaps.Points;
using YouMaps.TappedStates;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace YouMaps
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    [TemplatePart(Name = "PolyItemStyle", Type = typeof(MapPolyline))]
    public sealed partial class MapPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private MapControl.Location currentLocation;
        private LoadMap loadMap;
        private Object tappedObject;
        private Point? startingPosition;
        
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

        Geolocator geo = null;
        public async void Navigate()
        {
            if ((App.Current as App).SavedMapLoading != null)
            {
                loadMap = (App.Current as App).SavedMapLoading;
            }
            else
            {
                currentLocation = (App.Current as App).CurrentLocation;
                loadMap = new LoadMap(currentLocation);
                // loadMap.MapCenter = currentLocation;
            }
            MapGrid.DataContext = loadMap;

        }

       
        public MapPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            (App.Current as App).CurrentTappedState = new NormalState();
            myMap.Holding += new HoldingEventHandler(MyMap_Holding);
            myMap.Tapped += tappedPointer;
            myMap.PointerPressed += drawingPointerIsPressed;
            myMap.PointerReleased += drawingPointerReleased;
            myMap.PointerMoved += drawingPointerHasMoved;
       
           
            ChangeRedColor.ValueChanged += ChangeColorLine;
            ChangeBlueColor.ValueChanged += ChangeColorLine;
            ChangeGreenColor.ValueChanged += ChangeColorLine;
            
           

          
            
        }

       

        private void tappedPointer(object sender, TappedRoutedEventArgs e)
        {
            Point pp = e.GetPosition(myMap);
                MapControl.Location location = myMap.ViewportPointToLocation(pp);
                if ((App.Current as App).CurrentSymbolToBePlaced == null)
                {
                    (App.Current as App).CurrentTappedState.Exacute(tappedObject, loadMap, location);
                }
                else
                {

                    if ((App.Current as App).CurrentSymbolToBePlaced != null && (App.Current as App).PlaceSymbolOnTap)
                    {
                        (App.Current as App).PlaceSymbolOnTap = false;
                        YouMapsSymbol symb = (App.Current as App).CurrentSymbolToBePlaced;
                        DrawSymbolOnMap(symb,e);
                        
                        (App.Current as App).CurrentSymbolToBePlaced = null;
                    }
                    else if ((App.Current as App).CurrentSymbolToBePlaced != null)
                    {
                        (App.Current as App).PlaceSymbolOnTap = true;
                    }
                }
           }
        

        private bool drawingPointerIsOn = false;
        private bool drawingPressedIsOn = false;

        public static int locationInLocationsArray = 0;
       

        private void drawingPointerHasMoved(object sender, PointerRoutedEventArgs e)
        {

            if (drawingPointerIsOn && drawingPressedIsOn)
            {
                
                
                PointerPoint pp = e.GetCurrentPoint(myMap);
                MapControl.Location location = myMap.ViewportPointToLocation(pp.Position);
                MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                StyleUserControl sty = new StyleUserControl(LineColorBrush);
                SharpKml.Dom.CoordinateCollection coordCollection = new SharpKml.Dom.CoordinateCollection();

                     
                loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection, ColorOfLine = LineColorBrush, LocationAsCords = coordCollection});
                loadMap.Polylines.ElementAt(locationInLocationsArray).Locations.Add(location);
                loadMap.Polylines.ElementAt(locationInLocationsArray).LocationAsCords.Add(new SharpKml.Base.Vector { Latitude = location.Latitude, Longitude = location.Longitude });

                
            }
            else
            {
                if (startingPosition.HasValue)
                {
                    var postion = e.GetCurrentPoint(myMap).Position;
                    
                    myMap.TranslateMap(new Point(postion.X - startingPosition.Value.X, postion.Y - startingPosition.Value.Y));
                    startingPosition = postion;
                }
            }
        }

        private async void DrawSymbolOnMap(YouMapsSymbol symbol, TappedRoutedEventArgs e)
        {
            Point pp = e.GetPosition(myMap);

            


            AddSymbolToMap astm = new AddSymbolToMap(pp);

            List<PointCollection> pCollect = await astm.ChangePointToPlaceSymbol();
            foreach(PointCollection pc in pCollect)
            {
                SharpKml.Dom.CoordinateCollection coordCollection = new SharpKml.Dom.CoordinateCollection();
                MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection, ColorOfLine = LineColorBrush, LocationAsCords = coordCollection });

                foreach(Point p in pc)
                {
                    
                    MapControl.Location location = myMap.ViewportPointToLocation(p);
                   // StyleUserControl sty = new StyleUserControl(LineColorBrush);

                    loadMap.Polylines.ElementAt(locationInLocationsArray).Locations.Add(location);
                    loadMap.Polylines.ElementAt(locationInLocationsArray).LocationAsCords.Add(new SharpKml.Base.Vector { Latitude = location.Latitude, Longitude = location.Longitude });

                }
                locationInLocationsArray++;
            }

        }

        private void MapStyle()
        {


            //StyleUserControl suc = new StyleUserControl(LineColorBrush);
            //PTemplate.ItemTemplate = itemTemplate;
            //PStyle.Style = stylePoly;
            //MapPolyline polyLine = new MapPolyline();
           // this.pageRoot.C

            //DataTemplate polyLineItemTemplate = new DataTemplate();
            //ControlTemplate controlTemplate = new ControlTemplate();
            //controlTemplate.TargetType = typeof(MapItem);
            //var mapPolyLineFrameWorkElement = new FrameworkElement(typeof(MapPolyline));
            //Style style = new Style();
            //style.TargetType = typeof(MapItem);
            //Setter setter = new Setter();
            //setter.Property = MapItem.TemplateProperty;
            //style.Setters.Add(setter);
            ////controlTemplate.
            //setter.Value = controlTemplate;
            //style.Setters.Add(new Setter(MapPolyline.StrokeThicknessProperty, 3));
            //Binding Locationbind = new Binding();
            //Binding ColorBind = new Binding();
            //Locationbind.ElementName = "Location";
            //ColorBind.ElementName = "ColorOfLine";
            //ColorBind.Source = MapPolyline.
            //BindingOperations.SetBinding(style, Points.YouMapPolyline.Locations, Locationbind);
            
            //ItemControlToTest.Style = style;

        }

        private void drawingPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (drawingPointerIsOn && drawingPressedIsOn)
            {
         
                drawingPressedIsOn = false;
                locationInLocationsArray++;
         
            }
            else
            {
                startingPosition = null;
            }
        }

        private void drawingPointerIsPressed(object sender, PointerRoutedEventArgs e)
        {
            if (drawingPointerIsOn && !drawingPressedIsOn)
            {
               
                drawingPressedIsOn = true;

            }
            else
            {
                startingPosition = e.GetCurrentPoint(myMap).Position;
            }
        }

        private void InitializeMap()
        {
           //myMap.Center = new Location()    
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
            Navigate();
            //GetCurrentGPSLocation();
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
        
        
        public event PropertyChangedEventHandler PropertyChanged;

     

        //protected void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //        handler(this, e);
        //}
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        //}

        MapControl.Location pressedLocation = new MapControl.Location();
        void MyMap_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!drawingPointerIsOn) 
            {  
                 pressedLocation = myMap.ViewportPointToLocation(e.GetPosition(myMap));
            }
            else
            {
                MapGrid.CancelDirectManipulations();
            }
                      
            
        }
        private void AddPointer(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Button button = new Button();
            button.Click += PlacePoint;
            button.FontSize = 25;
            button.Content = "Ready To Place Point";
            button.SetValue(Grid.RowProperty, 1);
            button.Background = new SolidColorBrush(Colors.Gray);

            TextBox textBox = new TextBox();
            textBox.PlaceholderText = "Enter name, leave blank for GPS cords";
            textBox.FontSize = 25;
            textBox.SetValue(Grid.RowProperty, 0);
            grid.Children.Add(button);
            grid.Children.Add(textBox);
            MapPopup.Child = grid;
            MapPopup.IsOpen = true;
            


        }

        private void AddNote(object sender, RoutedEventArgs e)
        {
            Location location = new Location { Latitude = pressedLocation.Latitude, Longitude = pressedLocation.Longitude };
            loadMap.Images.Add(new Points.ImagePoint { Name = "This Image", WebURL = "A URL", Location = location  });

        }

        private void DrawOnMap(object sender, RoutedEventArgs e)
        {
            drawingPointerIsOn = true;
           
            
                        
        }

        bool symbolsVisable = false;
        private async void Symbols(object sender, RoutedEventArgs e)
        {
            symbolsVisable = !symbolsVisable;

           Visibility visabiltiy = new Visibility();
           visabiltiy =  (symbolsVisable) ? Visibility.Visible : Visibility.Collapsed;
           SymbolsStackPanel.Visibility = visabiltiy;
           Brush brush = new SolidColorBrush(Colors.Gray);
           List<YouMapsSymbol> symbolsOnList = await IOFile.getSucOnList();
           SymbolsStackPanel.Children.Clear();
            
            foreach(YouMapsSymbol yms in symbolsOnList)
            {
                Button button = new Button();
                button.Content = yms.Name;
                button.Click += symbolButtonclicked;
                button.DataContext = yms;
                button.Background = brush;
                button.Width = 100;
                SymbolsStackPanel.Children.Add(button);
            }
            
            
        }

        private void symbolButtonclicked(object sender, RoutedEventArgs e)
        {            
            var dataContext = ((Button)sender).DataContext;
            (App.Current as App).CurrentSymbolToBePlaced = (YouMapsSymbol)dataContext;
            (App.Current as App).PlaceSymbolOnTap = false;


        }

        private void ManageYouMapsSymbols(object sender, RoutedEventArgs e)
        {


            (App.Current as App).SavedMapLoading = loadMap;
            this.Frame.Navigate(typeof(ManageSymbols));
            //ManageYouMapsSymbolsPopup.IsOpen = true;

        }
        private Brush lineColorBrush = new SolidColorBrush(Colors.Black);

        public Brush LineColorBrush
        {
            get { return lineColorBrush; }
            set
            {
                //if (value != null && !value.Equals(lineColorBrush))
        {
            //lineColorBrush = value;
                //OnPropertyChanged("LineColorBrush");
        }} }

        private void ChangeColorLine(object sender, RangeBaseValueChangedEventArgs e)
        {
            
            
            int alpha = 200;
            Color tempColor = new Color();

            tempColor.A = (byte)alpha;
            tempColor.B = (byte)ChangeBlueColor.Value;
            tempColor.G = (byte)ChangeGreenColor.Value;
            tempColor.R = (byte)ChangeRedColor.Value;
            Brush brush = new SolidColorBrush(tempColor);
            LineColorBrush = new SolidColorBrush(tempColor);
            ChangeRedColor.Background = brush;
            ChangeGreenColor.Background = brush;
            ChangeBlueColor.Background = brush;
            

            
            
            
        }

        private void SaveF(object sender, RoutedEventArgs e)
        {
            ConvertToKml ctk = new ConvertToKml(loadMap);
            ctk.ConvertFileToKml();

        }

        private void DoneDrawing(object sender, RoutedEventArgs e)
        {
            drawingPointerIsOn = false;
            ConvertToKml ctk = new ConvertToKml(loadMap);

        }

        private void EditPointsAndNotes(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollview = new ScrollViewer();            
            scrollview.Background = new SolidColorBrush(Colors.AntiqueWhite);
            Grid grid = new Grid();
            scrollview.MaxHeight = 300;
            

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            scrollview.Content = grid;
            int count = 0;
            List<Button> listOfItemsOnMap = new List<Button>();
            foreach(YouMapPoint ymp in loadMap.Points)
            {
                Button button = new Button();
                
                grid.RowDefinitions.Add(new RowDefinition());
                button.SetValue(Grid.ColumnProperty, 0);
                button.SetValue(Grid.RowProperty, count);
                button.Background = new SolidColorBrush(Colors.Gray);
                
                button.FontSize = 25;  
                button.DataContext = ymp;
                button.Content = ymp.Name;
                button.Click += button_Click;
                listOfItemsOnMap.Add(button);
                count++;
                grid.Children.Add(button);


            }
           
            foreach(ImagePoint ip in loadMap.Images)
            {
                Button button = new Button();
                grid.RowDefinitions.Add(new RowDefinition());
                button.SetValue(Grid.ColumnProperty, 0);
                button.SetValue(Grid.RowProperty, count); ;
                button.DataContext = ip;
                button.Background = new SolidColorBrush(Colors.Gray);
                button.FontSize = 25;
                button.Content = ip.Name;
                button.Click += button_Click;
                listOfItemsOnMap.Add(button);
                count++;
                grid.Children.Add(button);

            }
           
            
            MapPopup.Child = scrollview;
            if (grid.Children.Count > 0)
            {
                MapPopup.IsOpen = true;
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            var type = ((Button)sender).DataContext;
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            TextBox name = new TextBox();
            name.SetValue(Grid.RowProperty, 0);
            TextBox other = new TextBox();
            Button button = new Button();
            button.Background = new SolidColorBrush(Colors.Gray);
            button.Content = "Save Changes";
            button.SetValue(Grid.RowProperty, 2);
            other.SetValue(Grid.RowProperty, 1);
            grid.Children.Add(name);
            grid.Children.Add(other);
            grid.Children.Add(button);
            button.Click += saveChangesToPointObj;



            if(type is YouMapPoint)
            {
                YouMapPoint ymp = type as YouMapPoint;
                
                name.Text = ymp.Name;
                button.DataContext = ymp;
                MapPopup.Child = grid;
            }
            else if(type is NotePoint)
            {
                NotePoint np = type as NotePoint;
                name.Text = np.Name;
                other.Text = np.Content;
                button.DataContext = np;
                MapPopup.Child = grid;
            }
            else
            {
                ImagePoint ip = type as ImagePoint;
                name.Text = ip.Name;
                other.Text = ip.WebURL;
                button.DataContext = ip;
                MapPopup.Child = grid;
            }
        }

        private void saveChangesToPointObj(object sender, RoutedEventArgs e)
        {
            Button asButton = (Button)sender;
            var type = ((Button)sender).DataContext;
            Grid parent = (Grid)asButton.Parent;
            var nameBox = parent.Children.ElementAt(0);
            var otherBox = parent.Children.ElementAt(1);
            if (type is YouMapPoint)
            {
                YouMapPoint ymp = type as YouMapPoint;
                YouMapPoint tempYouP = loadMap.Points.First(x => x.Equals(ymp));
                tempYouP.Name = ((TextBox)nameBox).Text;
                
                
            }
            else
            {
                ImagePoint ip = type as ImagePoint;
                ImagePoint tempP = loadMap.Images.First(x => x.Equals(ip));
                tempP.Name = ((TextBox)nameBox).Text;
                tempP.WebURL = ((TextBox)otherBox).Text;
               
            }
            MapPopup.IsOpen = false;
          
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            Grid grid = new Grid();
            Brush brush = new SolidColorBrush(Colors.Gray);
            Button placePic = new Button();
            TextBox name = new TextBox();
            TextBox URL = new TextBox();
            placePic.Content = "Ready To Place Picture";
            placePic.Click += PlacePicture;
            placePic.Background = brush;
            name.PlaceholderText = "Name of Picture";
            URL.PlaceholderText = "Write URL here";
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            name.SetValue(Grid.RowProperty, 0);
            URL.SetValue(Grid.RowProperty, 1);
            placePic.SetValue(Grid.RowProperty, 2);

            grid.Children.Add(placePic);
            grid.Children.Add(name);
            grid.Children.Add(URL);

            MapPopup.Child = grid;
            MapPopup.IsOpen = true;


            
            //FrameworkElement tempFrame = sender as FrameworkElement;
            //StackPanel stackParent = (StackPanel)tempFrame.Parent;
            //Grid tempParent = (Grid)stackParent.Parent;
            //FlyoutBase.ShowAttachedFlyout(tempParent);

        }

        private void PlacePicture(object sender, RoutedEventArgs e)
        {
            Button tempButton = sender as Button;
            Grid tempGrid = tempButton.Parent as Grid;           
            Popup temp = tempGrid.Parent as Popup;
            temp.IsOpen = false;
            
            TextBox nameTextBox;
            //TextBox URLTextBox;

            try
            {
                nameTextBox = tempGrid.Children.ElementAt(1) as TextBox;              
                TextBox otherBox = tempGrid.Children.ElementAt(2) as TextBox;
                string name = nameTextBox.Text;
                string URL = otherBox.Text;
                tappedObject = new ImagePoint { Name = name,WebURL = URL,Location = null };
                (App.Current as App).CurrentTappedState = new ImagePlacementState();

            }
            catch(Exception ex)
            {

            }
        }

        private void PlacePoint(object sender, RoutedEventArgs e)
        {
            YouMapPoint ymp = new YouMapPoint();
            
            Grid parentGrid = (Grid)((Button)sender).Parent;
           
            ymp.Name = ((TextBox)parentGrid.Children.ElementAt(1)).Text;
            (App.Current as App).CurrentTappedState = new PointTappedState();
            tappedObject = ymp;
            MapPopup.IsOpen = false;

            


        }

        private void ClearMap(object sender, RoutedEventArgs e)
        {
            (App.Current as App).SavedMapLoading = null;
            (App.Current as App).SavedMapLoading = new LoadMap((App.Current as App).CurrentLocation);
            Navigate();
        }

       

        

             

        //private MapControl.Location mapCenter;
        //public MapControl.Location MapCenter
        //{
        //    get { return mapCenter; }
        //    set
        //    {
        //        mapCenter = value;
        //        OnPropertyChanged("MapCenter");
        //    }
        //}
    }
}
