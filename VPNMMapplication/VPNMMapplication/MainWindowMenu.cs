using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


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
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            //for (int i = 0; i < mM_MK_UnitDataGrid.get; i++)
            //{
            //    for (int j = 0; j < mM_MK_UnitDataGrid.Columns[1].r; j++)
            //    {
            //        ExcelApp.Cells[i + 1, j + 1] = mM_MK_UnitDataGrid.Rows[i].Cells[j].Value;
            //    }
            //}
            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
        }

        private void menuToXmlForRMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = "ImportToRMS.xml";
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                saveFile.Filter = "Файл импорта (*.xml) | *.xml";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    pathToFile = saveFile.FileName;
                SaveToXML.BuildXmlDoc(fullCollection, pathToFile);

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
    }
}
