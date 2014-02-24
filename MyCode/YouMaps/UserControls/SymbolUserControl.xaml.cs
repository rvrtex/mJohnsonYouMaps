using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using YouMaps.Symbols;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace YouMaps.UserControls
{
    public sealed partial class SymbolUserControl : UserControl
    {
        YouMapsSymbol youmapsSymbol;
        Panel panelObject;
        public CheckBox checkbox;
        Page page;
        Canvas drawingCanvas;
        public SymbolUserControl(YouMapsSymbol youmapsSymbol, Panel panelObject, Page page, Canvas drawingCanvas)
        {
            this.InitializeComponent();
            this.youmapsSymbol = youmapsSymbol;
            SymbolButton.Content = youmapsSymbol.Name;
            this.panelObject = panelObject;
            this.page = page;
            this.drawingCanvas = drawingCanvas;
            checkbox = CheckBoxForEverything;
            
        }
        Polyline polyLine;
        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            
            (App.Current as App).CurrentSymbol  = youmapsSymbol;
            
            foreach(PointCollection p in youmapsSymbol.SymbolPoints)
            {
                double thickness = 10;

                polyLine = new Polyline { Stroke = new SolidColorBrush { Color = Colors.Black }, StrokeThickness = thickness };
                polyLine.Points = p;
                drawingCanvas.Children.Add(polyLine);
                
            }
            ((ManageSymbols)page).AddSymbol(sender,e);
        }
    }
}
