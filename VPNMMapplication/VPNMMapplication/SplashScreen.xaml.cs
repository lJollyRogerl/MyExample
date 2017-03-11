using Microsoft.Win32;
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

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        //private void btnPickFile_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog openFile = new OpenFileDialog();
        //    openFile.Filter = "html страница (*.html) | *.html";
        //    if (openFile.ShowDialog() == DialogResult.OK)
        //    {
        //        pathToFile = openFile.FileName;
        //        load = new LoadFromFile(pathToFile);
        //        txtBox.Text = pathToFile;
        //    }
        //}
    }
}
