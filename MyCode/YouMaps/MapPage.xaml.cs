﻿using YouMaps.Common;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace YouMaps
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
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
            //myMap.
            myMap.PointerPressed += drawingPointerIsPressed;
            myMap.PointerReleased += drawingPointerReleased;
            myMap.PointerMoved += drawingPointerHasMoved;
            //myMap.PointerEntered += drawingPointerStartingObject;
            //myMap.PointerExited += drawingPointerExitedObject;
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
                //myMap.CancelDirectManipulations();

               // MapBase myBase = myMap;
                AddPointPopup.IsOpen = false;
                PointerPoint pp = e.GetCurrentPoint(myMap);
                MapControl.Location location = myMap.ViewportPointToLocation(pp.Position);
                MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection });
                loadMap.Polylines.ElementAt(locationInLocationsArray).Locations.Add(location);

            }
        }

        private void drawingPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (drawingPointerIsOn && drawingPressedIsOn)
            {
               // MapGrid.CancelDirectManipulations();
               
                drawingPressedIsOn = false;
                locationInLocationsArray++;

                //AddPointPopup.IsOpen = false;
                //PointerPoint pp = e.GetCurrentPoint(myMap);
                //var location = myMap.ViewportPointToLocation(pp.Position);
                //MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                //locationCollection.Add(location);
                //loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection });
            }
        }

        private void drawingPointerIsPressed(object sender, PointerRoutedEventArgs e)
        {
            if (drawingPointerIsOn && !drawingPressedIsOn)
            {
                //MapGrid.CancelDirectManipulations();
                drawingPressedIsOn = true;
                

                //PointerPoint pp = e.GetCurrentPoint(myMap);
                //var location = myMap.ViewportPointToLocation(pp.Position);
                //MapControl.LocationCollection locationCollection = new MapControl.LocationCollection();
                //locationCollection.Add(location);
                //loadMap.Polylines.Add(new Points.YouMapPolyline { Locations = locationCollection });
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

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
            //MapGrid.CancelDirectManipulations();
            //loadMap.Polylines.Add(
            //new Points.YouMapPolyline
            // {
            //        Locations = MapControl.LocationCollection.Parse("53.5150,8.1451 53.5123,8.1506 53.5156,8.1623 53.5276,8.1757 ")
            //    });
            //            loadMap.Polylines.Add(
            //                new Points.YouMapPolyline
            //                {
            //                    Locations = MapControl.LocationCollection.Parse("53.5978,8.1212 53.6018,8.1494 53.5859,8.1554 53.5852,8.1531 53.5841,8.1539 53.5802,8.1392 53.5826,8.1309 53.5867,8.1317 53.5978,8.1212")
            //                });
                        
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
            ManageYouMapsSymbolsPopup.IsOpen = true;

        }

        private void AddSymbol(object sender, RoutedEventArgs e)
        {

        }

        private void EditSymbol(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSymbol(object sender, RoutedEventArgs e)
        {

        }

        private void AddSymbolEasyAccess(object sender, RoutedEventArgs e)
        {

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
