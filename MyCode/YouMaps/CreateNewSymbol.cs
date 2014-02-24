using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace YouMaps
{
    class CreateNewSymbol
    {
        private MapPage mapPage;
        Canvas DrawingCanvas;

        

        public CreateNewSymbol(Canvas drawingCanvas)
        {
                this.DrawingCanvas = drawingCanvas;

            
                // TODO: Complete member initialization
               
            
        }


        private void drawingPointerHasMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            
            //CanvasPolylines.Add
        }

        private void drawingPointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        
       
        private void drawingPointerIsPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            
        }


    }
}
