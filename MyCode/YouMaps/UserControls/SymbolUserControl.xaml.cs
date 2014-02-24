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
        public YouMapsSymbol YouMapsSymbol { get; set; } 
        
       
        
        public CheckBox CheckBoxEverything { get; set; }
        public TextBlock SymbolNameText { get; set; }
        public RadioButton EditRadioButton { get; set; }
        
        public SymbolUserControl(YouMapsSymbol youmapsSymbol)
        {
            CheckBoxEverything = new CheckBox();
            SymbolNameText = new TextBlock();
            EditRadioButton = new RadioButton();

            this.InitializeComponent();
            this.YouMapsSymbol = youmapsSymbol;
            SymbolNameText.Text = youmapsSymbol.Name;
            SymbolNameText.FontSize = 30;
            ControlStackpanel.Children.Add(CheckBoxEverything);
            ControlStackpanel.Children.Add(SymbolNameText);
            ControlStackpanel.Children.Add(EditRadioButton);


           
            
            
        }
        
        
    }
}
