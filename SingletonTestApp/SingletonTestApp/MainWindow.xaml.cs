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
using System.Configuration;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Entity;

namespace SingletonTestApp
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["connectionToAutoLot"].ConnectionString;
            //SqlConnection con = ConnectionSetter.GetConnection(connectionString);
            try
            {
                AutoLotEntities ent = new AutoLotEntities();
                //Inventory inv = new Inventory();
                //inv.CarID = 20;
                //inv.Color = "Коричневый";
                //inv.Make = "Рено";
                //inv.PetName = "Дастер";
                //ent.Inventory.Add(inv);
                //ent.SaveChanges();
                ent.Inventory.Load();
                dataGrid.ItemsSource = ((DbSet)ent.Inventory).Local;

                MessageBox.Show("Готово");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
