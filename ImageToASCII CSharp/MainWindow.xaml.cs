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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace ImageToASCII_CSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Bitmap image;
        private int nWidth;
        private int nFontSize;

        private ASCIIWindow ASCIIArea;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            //Filter
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*jpg;)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                image = new Bitmap(openFileDialog.FileName);

                nWidth      = int.Parse(RatioBox.Text);
                nFontSize   = int.Parse(FontSize.Text);

                // Width je uvijek veci od 0
                if (nWidth <= 0)
                    nWidth = 1;

                // Font je uvijek veci od 0
                if (nFontSize <= 0)
                    nFontSize = 1;

                ImageToASCII();

                //Make finish text visible & ShowOutputButton visible
                FinishText.Visibility = Visibility.Visible;
                ShowOutput.Visibility = Visibility.Visible;
            }
            else
            {
                //Error message box
                MessageBox.Show("Unexpected Error", ":(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImageToASCII()
        {
            ASCIIArea = new ASCIIWindow();

            StreamWriter writer = File.CreateText("asciiArt.txt");

            double redPart = 0.299;
            double greenPart = 0.587;
            double bluePart = 0.114;

            /*
                   * The greyscale value is calculated according to
                   * the following formula
                   * GREY = 0.299 * RED + 0.587 * GREEN + 0.114 * BLUE
                   * For more information see http://en.wikipedia.org/wiki/Grayscale
            */

            //Vector zapravo
            List<double> ValueHolder = new List<double>();

            string art = String.Empty;

            //Looping through pixels
            for (int i = 0; i < (int)image.Height / nWidth; i++)
            {
                for (int j = 0; j < (int)image.Width / nWidth; j++)
                {
                    System.Drawing.Color pixelColor;
                    double greyShade;

                    for (int k = 0; k < nWidth; k++)
                    {
                        for (int l = 0; l < nWidth; l++)
                        {
                            //Gleda sve piksele desno i ispod toga piksela koji je pocetni
                            pixelColor = image.GetPixel(j * nWidth + k, i * nWidth + l);

                            greyShade = redPart * (double)pixelColor.R + greenPart * (double)pixelColor.G + bluePart * (double)pixelColor.B;

                            ValueHolder.Add(greyShade);
                        }
                    }

                    //Get average
                    greyShade = ValueHolder.Average();

                    //Write to .txt
                    writer.Write(GetSymbol(greyShade));
                    art += GetSymbol(greyShade)[0];

                    //Clear list
                    ValueHolder.Clear();
                }
                //New line
                writer.WriteLine();
                art += '\n';
            }

            //Close .txt document
            writer.Close();

            ASCIIArea.Show();
            ASCIIArea.DrawASCIIArt(art, image, nWidth, nFontSize);
        }

        private string GetSymbol(double l_greyShade)
        {
            if (l_greyShade >= 230)
                return " ";
            else if (l_greyShade >= 205)
                return ".";
            else if (l_greyShade >= 190)
                return "-";
            else if (l_greyShade >= 165)
                return ":";
            else if (l_greyShade >= 140)
                return "=";
            else if (l_greyShade >= 115)
                return "+";
            else if (l_greyShade >= 90)
                return "x";
            else if (l_greyShade >= 65)
                return "#";
            else if (l_greyShade >= 40)
                return "%";
            else if (l_greyShade >= 0)
                return "@";

            return " ";
        }

        private void FinishText_Initialized(object sender, EventArgs e)
        {
            //On start label is invisible
            FinishText.Visibility = Visibility.Hidden;
        }

        private void ShowOutput_Initialized(object sender, EventArgs e)
        {
            //On start button is invisible
            ShowOutput.Visibility = Visibility.Hidden;
        }

        private void ShowOutput_Click(object sender, RoutedEventArgs e)
        {
            //Open asciiArt.txt
            Process.Start("notepad.exe", "asciiArt.txt");
        }

        // If the main window is closed close the ASCIIArea window as well
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ASCIIArea != null)
            {
                ASCIIArea.Close();
            }
        }
    }
}
