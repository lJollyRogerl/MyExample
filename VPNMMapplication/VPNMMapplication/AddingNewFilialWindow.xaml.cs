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
    /// Interaction logic for AddingNewFilialWindow.xaml
    /// </summary>
    public partial class AddingNewFilialWindow : Window
    {
        Divisions divisions;
        public AddingNewFilialWindow()
        {
            InitializeComponent();
        }
        public AddingNewFilialWindow(Divisions div)
        {
            divisions = div;
            InitializeComponent();
        }

        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {
            SerializeDivisions.AddFillial(divisions, 
                new Filial(txtFilialName.Text, new Region(txtOwnerRegion.Text)));
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
