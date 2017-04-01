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
                saveFile.Filter = "Таблица (*.csv) | *.csv";
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
                    SaveToXMLForRMS.BuildXmlDoc(fullCollection, pathToFile);
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

        private void helpDesсription_Click(object sender, RoutedEventArgs e)
        {
            //Написать код, который будет выводить окно с подсказками
        }

        private void helpWriteToDeveloper_Click(object sender, RoutedEventArgs e)
        {
            string subject = "Предложения и пожелания VPNMMapplication";
            Process.Start("mailto:dmitriev_gv@ntagil.magnit.ru" + "?subject=" + subject/* + "&body="+ body*/);
        }

        private void comboWhatToShow_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((string)comboWhatToShow.SelectedValue == "Мониторинг")
            {
                ////Написать код, который будет показывать только мониторинг и соответсвующие контролы
                //statusesDataGrid.Visibility = Visibility.Visible;
                //mM_MK_UnitDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ////Написать код, который будет показывать только логи и соответсвующие контролы
                //statusesDataGrid.Visibility = Visibility.Collapsed;
                //mM_MK_UnitDataGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
