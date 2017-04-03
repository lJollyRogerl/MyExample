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
        private Settings settings = new Settings() { TimePerLogging = 4 };

        public SettingsWindow()
        {
            InitializeComponent();
        }

        public SettingsWindow(ref Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
        }

        private void sliderTimePerLoggingSet_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if((int)e.NewValue==1)
                lblTimePerLogging.Content = $"Логирование происходит каждый час";
            else
                lblTimePerLogging.Content = $"Логирование происходит каждые {(int)e.NewValue} часа";

            settings.TimePerLogging = ((int)e.NewValue);
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
