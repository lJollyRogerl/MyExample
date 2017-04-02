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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        int timePerLoggingSet = 4;
        private Settings settings;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(Settings settings)
        {
            this.settings = settings;
        }

        private void sliderTimePerLoggingSet_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblTimePerLogging.Content = $"Логирование происходит каждые {(int)e.NewValue} часа";
            timePerLoggingSet = ((int)e.NewValue);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
