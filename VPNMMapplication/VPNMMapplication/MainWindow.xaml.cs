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
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string readyObjects = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //грузим страницу
                string htmlText = File.ReadAllText(@"vpnmm\manage.htm", Encoding.UTF8);
                MM_MK_DictionarryMaker maker = new MM_MK_DictionarryMaker(htmlText, true);

                //Выводим на текстбокс всю выборку имен ММ/МК
                
                foreach (var mm_mk in maker.MM_MK_Dictionary)
                {
                    readyObjects += mm_mk.Key+" - "+mm_mk.Value;
                    readyObjects += "\n";
                }
                txtAllText.Text = readyObjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            
        }

    }
}
