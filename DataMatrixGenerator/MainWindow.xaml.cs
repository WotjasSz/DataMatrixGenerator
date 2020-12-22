using DataMatrix.net;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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

namespace DataMatrixGenerator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variables        
        Dictionary<string, int> asciiDictionary = new Dictionary<string, int>();
        ObservableCollection<string> dataMatrixDataList;

        public MainWindow()
        {
            InitializeComponent();

            GenerateAsciiDictionary();
            ButtonGenerate();

            txtPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dataMatrixDataList = new ObservableCollection<string>();            
            lstData.ItemsSource = dataMatrixDataList;
        }
        #region Event Handlers
        private void btnPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            CommonFileDialogResult result = dialog.ShowDialog();

            txtPath.Text = dialog.FileName;            
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(txtPath.Text))
            {
                int i = 1;
                foreach (string s in dataMatrixDataList)
                {
                    string fullFileName = string.Format("{0}\\DM{1:D4}.png", txtPath.Text, i); //[)><RS>06<GS>P40217372<GS>1T20347[0000]<RS><EOT>       
                    DmtxImageEncoder encoder = new DmtxImageEncoder();
                    Bitmap bmp = encoder.EncodeImage(s);
                    bmp.Save(fullFileName, System.Drawing.Imaging.ImageFormat.Png);
                    i++;
                    txtResults.AppendText(StringConverting.DisplayString(s));
                }
            }
            else
            {
                MessageBox.Show("Wybrana ścieżka nie istnieje!!!", "Zła ścieżka", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dataMatrixDataList.Add(ConvertToAsciiString(txtContent.Text));            
            txtContent.Text = "";
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Enter)
            {
                dataMatrixDataList.Add(ConvertToAsciiString(txtContent.Text));                
                txtContent.Text = "";
            }
        }

        private void btnSpecialAscii(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Content)
            {
                case "NUL":
                    txtContent.AppendText("<NUL>");                    
                    break;
                case "SOH":
                    txtContent.AppendText("<SOH>");
                    break;
                case "STX":
                    txtContent.AppendText("<STX>");
                    break;
                case "ETX":
                    txtContent.AppendText("<ETX>");
                    break;
                case "EOT":
                    txtContent.AppendText("<EOT>");
                    break;
                case "ENQ":
                    txtContent.AppendText("<ENQ>");
                    break;
                case "ACK":
                    txtContent.AppendText("<ACK>");
                    break;
                case "BEL":
                    txtContent.AppendText("<BEL>");
                    break;
                case "BS":
                    txtContent.AppendText("<BS>");
                    break;
                case "HT":
                    txtContent.AppendText("<HT>");
                    break;
                case "LF":
                    txtContent.AppendText("<LF>");
                    break;
                case "VT":
                    txtContent.AppendText("<VT>");
                    break;
                case "FF":
                    txtContent.AppendText("<FF>");
                    break;
                case "CR":
                    txtContent.AppendText("<CR>");
                    break;
                case "SO":
                    txtContent.AppendText("<SO>");
                    break;
                case "SI":
                    txtContent.AppendText("<SI>");
                    break;
                case "DLE":
                    txtContent.AppendText("<DLE>");
                    break;
                case "DC1":
                    txtContent.AppendText("<DC1>");
                    break;
                case "DC2":
                    txtContent.AppendText("<DC2>");
                    break;
                case "DC3":
                    txtContent.AppendText("<DC3>");
                    break;
                case "DC4":
                    txtContent.AppendText("<DC4>");
                    break;
                case "NAK":
                    txtContent.AppendText("<NAK>");
                    break;
                case "SYN":
                    txtContent.AppendText("<SYN>");
                    break;
                case "ETB":
                    txtContent.AppendText("<ETB>");
                    break;
                case "CAN":
                    txtContent.AppendText("<CAN>");
                    break;
                case "EM":
                    txtContent.AppendText("<EM>");
                    break;
                case "SUB":
                    txtContent.AppendText("<SUB>");
                    break;
                case "ESC":
                    txtContent.AppendText("<ESC>");
                    break;
                case "FS":
                    txtContent.AppendText("<FS>");
                    break;
                case "GS":
                    txtContent.AppendText("<GS>");
                    break;
                case "RS":
                    txtContent.AppendText("<RS>");
                    break;
                case "US":
                    txtContent.AppendText("<US>");
                    break;
                default:
                    break;
            }
        }

        private void btnGenerateSeries_Click(object sender, RoutedEventArgs e)
        {
            int serieSize = 0;
            int startCount = int.Parse(txtStartNo.Text);
            int stopCount = int.Parse(txtStopNo.Text);
            string newDataMatrixString = txtContent.Text;
            if (int.TryParse(txtCounterSize.Text, out serieSize))
            {
                for(int i = startCount; i <= stopCount; i++ )
                {
                    string counterValue = i.ToString(string.Format("D{0}", serieSize));
                    dataMatrixDataList.Add(ConvertToAsciiString(newDataMatrixString.Replace(CounterFormat(serieSize), counterValue)));
                }

            }

        }

        private void btnInsertCounter_Click(object sender, RoutedEventArgs e)
        {
            int serieSize = 0;
            if (int.TryParse(txtCounterSize.Text, out serieSize) && txtCounterSize.Text != string.Empty)                
            {
                txtContent.AppendText(CounterFormat(int.Parse(txtCounterSize.Text)));
            }
        }

        private void btnClearList_Click(object sender, RoutedEventArgs e)
        {
            dataMatrixDataList.Clear();
            txtContent.Text = "";
            txtResults.Text = "";
        }
        #endregion

        #region Methods
        private void ButtonGenerate()
        {
            foreach(string s in StringConverting.LowNames)
            {
                Button newBtn = new Button();

                newBtn.Content = s;
                newBtn.Name = string.Format("{0}{1}", "btn", s);
                newBtn.Width = 35;
                newBtn.Height = 20;
                Thickness thickness = new Thickness();
                thickness.Left = 2;
                thickness.Right = 2;
                thickness.Top = 1;
                thickness.Bottom = 1;
                newBtn.Margin = thickness;

                newBtn.Click += btnSpecialAscii;

                wrpSpecialAscii.Children.Add(newBtn);
            }            
        }

        private void GenerateAsciiDictionary()
        {
            
            for(int i = 0;i < StringConverting.LowNames.Length; i++)
            {                
                asciiDictionary.Add(string.Format("<{0}>",StringConverting.LowNames[i]), i);                
            }            
        }

        private string ConvertToAsciiString(string text)
        {
            string newContent = text;
            foreach(string s in asciiDictionary.Keys)
            {
                string newString = "";
                newString = newString + Convert.ToChar(asciiDictionary[s]);
                newContent = newContent.Replace(s,newString);                
            }
            return newContent;
        }

        private string CounterFormat(int counterSize)
        {
            string counterFormat = "";            
            for (int i = 0; i < counterSize; i++)
            {
                counterFormat = counterFormat + "0";
            }

            return string.Format("[{0}]", counterFormat);
        }
        #endregion
    }
}
