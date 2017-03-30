using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace VPNMMapplication
{
    public partial class MainWindow : Window
    {

        private void menuNewSession_Click(object sender, RoutedEventArgs e)
        {
            InvokeNewSession();
        }

        private void InvokeNewSession()
        {
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            this.Close();
        }

        private void menuToExel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = "exportToExcel.сsv";
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                saveFile.Filter = "(*.csv) | *.csv";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = saveFile.FileName;
                    DataGrid dg = mM_MK_UnitDataGrid;
                    dg.SelectAllCells();
                    dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, dg);
                    dg.UnselectAllCells();
                    String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    StreamWriter swObj = new StreamWriter(pathToFile);
                    swObj.WriteLine(Clipboardresult);
                    swObj.Close();
                    Process.Start(pathToFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void menuToXmlForRMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = "ImportToRMS.xml";
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                saveFile.Filter = "Файл импорта (*.xml) | *.xml";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = saveFile.FileName;
                    SaveToXML.BuildXmlDoc(fullCollection, pathToFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void helpDesription_Click(object sender, RoutedEventArgs e)
        {

        }

        private void helpWriteToDeveloper_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
