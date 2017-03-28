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
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            this.Close();
        }

        private void menuToExel_Click(object sender, RoutedEventArgs e)
        {

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
