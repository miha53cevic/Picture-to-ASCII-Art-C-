using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Drawing;

namespace ImageToASCII_CSharp
{
    /// <summary>
    /// Interaction logic for ASCIIWindow.xaml
    /// </summary>
    public partial class ASCIIWindow : Window
    {
        public ASCIIWindow()
        {
            InitializeComponent();
        }

        public void DrawASCIIArt(string art, Bitmap img, int Grouping, int FontSize)
        {
            // Setup text area size
            textArea.Width  = img.Width * FontSize / Grouping * 0.75;
            textArea.Height = img.Height * FontSize / Grouping;

            textArea.FontSize = FontSize;
            textArea.Text = art;
        }
    }
}
