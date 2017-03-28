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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Файл импорта (*.xml) | *.xml";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string pathToFile = saveFile.FileName;
                    System.Windows.MessageBox.Show(pathToFile);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
