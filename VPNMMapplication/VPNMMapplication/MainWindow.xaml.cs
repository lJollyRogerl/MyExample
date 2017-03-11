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
        MM_MK_DictionarryMaker maker;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //грузим страницу
                await maker.LoadDictionaryAsync();

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

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lblStatus.Width = mainWindow.Width / 3 * 1;
            progressBar.Width = mainWindow.Width / 3 * 2;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            maker = new MM_MK_DictionarryMaker(@"vpnmm\manage.htm", Encoding.UTF8);
            maker.OnProgressChanged += Maker_OnProgressChanged;
        }

        private void Maker_OnProgressChanged(ProgressInfo obj)
        {
            progressBar.Maximum = obj.TotalSteps;
            progressBar.Value = obj.CurrentStep;
        }
    }
}
