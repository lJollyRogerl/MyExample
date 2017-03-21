using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        MM_MK_CollectionMaker maker;
        Uri 
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MM_MK_CollectionMaker dictionaryMaker)
        {
            maker = dictionaryMaker;
            InitializeComponent();
        }

        private void MM_MK_UnitDataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is MM_MK_Unit)
            {
                MM_MK_Unit unit = (MM_MK_Unit)e.Row.DataContext;
                if (unit.IsOnline == false)
                {
                    e.Row.Background = new SolidColorBrush(Color.FromRgb(245,0,41));
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadAsync();
        }

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            lblStatus.Width = mainWindow.Width / 3 * 1;
            progressBar.Width = mainWindow.Width / 3 * 2;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            maker.OnProgressChanged += Maker_OnProgressChanged;
            mM_MK_UnitDataGrid.LoadingRow += MM_MK_UnitDataGrid_LoadingRow;
            LoadAsync();
        }

        private void Maker_OnProgressChanged(ProgressInfo obj)
        {
            this.Dispatcher.Invoke(() => 
            {
                progressBar.Maximum = obj.TotalSteps;
                progressBar.Value = obj.CurrentStep;
                lblStatus.Content = "Обработка " + obj.CurrentMM_MK.ToString();
            });
        }

        public void VisibleProgressOn()
        {
            progressBar.Visibility = Visibility.Visible;
            lblStatus.Visibility = Visibility.Visible;
        }

        public void VisibleProgressOff()
        {
            progressBar.Visibility = Visibility.Hidden;
            lblStatus.Visibility = Visibility.Hidden;
        }

        private async void LoadAsync()
        {
            btnLoad.IsEnabled = false;
            try
            {
                //Перед нечалом загрузки - включаем видимость прогресс бара
                VisibleProgressOn();
                //грузим страницу
                MM_MK_Collection col = await maker.LoadCollectionAsync(true);
                col.AddCollection(await maker.LoadCollectionAsync(false));
                //После загрузки - выключаем видимость прогресс бара
                VisibleProgressOff();
                mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnLoad.IsEnabled = true;
        }
    }
}
