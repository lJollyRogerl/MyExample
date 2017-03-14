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
using System.Windows.Forms;

namespace WpfApplication1
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

        private void textBox_GotMouseCapture(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
                openFile.Filter = "html страница (*.html) | *.html";
                if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string pathToFile = openFile.FileName;
                    
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
