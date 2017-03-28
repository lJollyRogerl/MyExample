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
            SaveToXML.BuildXmlDoc(fullCollection, "file");
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
