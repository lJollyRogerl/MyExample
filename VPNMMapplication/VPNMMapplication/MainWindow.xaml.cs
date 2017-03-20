using System;
using System.Text;
using System.Windows;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        MM_MK_CollectionMaker maker;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MM_MK_CollectionMaker dictionaryMaker)
        {
            maker = dictionaryMaker;
            InitializeComponent();
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
            //txtAllText.Text = maker.HtmlString;
            LoadAsync();
            //System.Windows.Data.CollectionViewSource mM_MK_UnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("mM_MK_UnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // mM_MK_UnitViewSource.Source = [generic data source]
        }

        private void Maker_OnProgressChanged(ProgressInfo obj)
        {
            this.Dispatcher.Invoke(() => 
            {
                progressBar.Maximum = obj.TotalSteps;
                progressBar.Value = obj.CurrentStep;
                lblStatus.Content = "Обработка "+obj.CurrentMM_MK.ToString();
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
