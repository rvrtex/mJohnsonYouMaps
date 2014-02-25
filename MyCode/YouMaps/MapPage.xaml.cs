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
            currentLocation = (App.Current as App).CurrentLocation;
            loadMap = new LoadMap(currentLocation);
           // loadMap.MapCenter = currentLocation;
            MapGrid.DataContext = loadMap;
        }

        public MapPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            //currentLocation = new MapControl.Location();
            myMap.Holding += new HoldingEventHandler(MyMap_Holding);
            myMap.Tapped += tappedPointer;
            myMap.PointerPressed += drawingPointerIsPressed;
            myMap.PointerReleased += drawingPointerReleased;
            myMap.PointerMoved += drawingPointerHasMoved;
            ChangeRedColor.ValueChanged += ChangeColorLine;
            ChangeBlueColor.ValueChanged += ChangeColorLine;
            ChangeGreenColor.ValueChanged += ChangeColorLine;
            
           

            //myMap.PointerEntered += drawingPointerStartingObject;
            //myMap.PointerExited += drawingPointerExitedObject;
        }

        private void tappedPointer(object sender, TappedRoutedEventArgs e)
        {
           if((App.Current as App).CurrentSymbol != null)
           {
               (App.Current as App).CurrentSymbol = null;

           }
        }

        private bool drawingPointerIsOn = false;
        private bool drawingPressedIsOn = false;
        private int locationInLocationsArray = 0;
        private void drawingPointerExitedObject(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void drawingPointerStartingObject(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void drawingPointerHasMoved(object sender, PointerRoutedEventArgs e)
        {

            if (drawingPointerIsOn && drawingPressedIsOn)
            {
                
                AddPointPopup.IsOpen = false;
                PointerPoint pp = e.GetCurrentPoint(myMap);
                MapControl.Location location = myMap.ViewportPointToLocation(pp.Position);
                MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                StyleUserControl sty = new StyleUserControl(LineColorBrush);
              
                
                //Points.YouMapPolyline points = new Points.YouMapPolyline { Locations = locationCollection, ColorOfLine = LineColorBrush };
                loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection,ColorOfLine = LineColorBrush });
                //loadMap.Polylines.Add(points);
                loadMap.Polylines.ElementAt(locationInLocationsArray).Locations.Add(location);
                //SolidColorBrush brus2 = (SolidColorBrush)LineColorBrush;
               // Debug.WriteLine(brus2.Color);
                
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
        }

        private void drawingPointerIsPressed(object sender, PointerRoutedEventArgs e)
        {
            if (drawingPointerIsOn && !drawingPressedIsOn)
            {
               
                drawingPressedIsOn = true;
                

               
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
            if (!AddPointPopup.IsOpen && !drawingPointerIsOn) { AddPointPopup.IsOpen = true; 
            pressedLocation = myMap.ViewportPointToLocation(e.GetPosition(myMap));
            }
            else
            {
                MapGrid.CancelDirectManipulations();
            }
                      
            
        }
        private void AddPointer(object sender, RoutedEventArgs e)
        {
            loadMap.Points.Add(new Points.YouMapPoint { Name = pressedLocation.Longitude+" "+pressedLocation.Latitude, Location = pressedLocation });
            AddPointPopup.IsOpen = false;


        }

        private void AddNote(object sender, RoutedEventArgs e)
        {
            AddPointPopup.IsOpen = false;

        }

        private void DrawOnMap(object sender, RoutedEventArgs e)
        {
            drawingPointerIsOn = true;
            AddPointPopup.IsOpen = false;
            
                        
        }

        bool symbolsVisable = false;
        private void Symbols(object sender, RoutedEventArgs e)
        {
            symbolsVisable = !symbolsVisable;

           Visibility visabiltiy = new Visibility();
           visabiltiy =  (symbolsVisable) ? Visibility.Visible : Visibility.Collapsed;
           SymbolsStackPanel.Visibility = visabiltiy;
           
            
            
        }

        private void ManageYouMapsSymbols(object sender, RoutedEventArgs e)
        {
            
            AddPointPopup.IsOpen = false;
            this.Frame.Navigate(typeof(ManageSymbols));
            //ManageYouMapsSymbolsPopup.IsOpen = true;

        }
        private Brush lineColorBrush;

        public Brush LineColorBrush
        {
            get { return lineColorBrush; }
            set
            {
                if (value != null && !value.Equals(lineColorBrush))
        {
            lineColorBrush = value;
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
