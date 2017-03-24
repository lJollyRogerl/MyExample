using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private MM_MK_CollectionMaker maker;
        private HTMLWithAutorization htmlGetter;
        private MM_MK_Collection col;
        private bool? isOnlineMode = false;
        DispatcherTimer dispatcherTimer;
        MM_MK_Collection onlineCollection = new MM_MK_Collection();
        MM_MK_Collection offlineCollection = new MM_MK_Collection();
        MM_MK_Collection fullCollection = new MM_MK_Collection();

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MM_MK_CollectionMaker dictionaryMaker)
        {
            maker = dictionaryMaker;
            InitializeComponent();
        }

        public MainWindow(MM_MK_CollectionMaker dictionaryMaker, HTMLWithAutorization getterForRefresh, bool? isChecked)
        {
            isOnlineMode = isChecked;
            maker = dictionaryMaker;
            htmlGetter = getterForRefresh;
            InitializeComponent();
        }

        private void MM_MK_UnitDataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is MM_MK_Unit)
            {
                MM_MK_Unit unit = (MM_MK_Unit)e.Row.DataContext;
                if (unit.IsOnline == false)
                {
                    e.Row.Background = new SolidColorBrush(Color.FromRgb(245, 0, 41));
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
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
            //Если запущено в онлайн режиме - обновляет статус ММ каждые 5 минут
            if (isOnlineMode == true)
            {
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += delegate (object s, EventArgs eArgs) { Refresh(); };
                dispatcherTimer.Interval = new TimeSpan(0, 7, 0);
                dispatcherTimer.Start();
            }

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
            btnRefresh.IsEnabled = false;
            try
            {
                //Перед нечалом загрузки - включаем видимость прогресс бара
                VisibleProgressOn();
                //грузим страницу
                onlineCollection = await maker.LoadCollectionAsync(true, checkBoxShowDate.IsChecked);
                onlineCollection.TheCollection.Sort(new MMCollectionComparer());
                offlineCollection = await maker.LoadCollectionAsync(false, checkBoxShowDate.IsChecked);
                offlineCollection.TheCollection.Sort(new MMCollectionComparer());
                fullCollection.Clear();
                fullCollection.AddCollection(onlineCollection);
                fullCollection.AddCollection(offlineCollection);
                fullCollection.TheCollection.Sort(new MMCollectionComparer());
                if (radioBtnOnline.IsChecked == true)
                {
                    this.Dispatcher.Invoke(() => {
                        col = onlineCollection;
                        mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
                    });
                }
                if (radioBtnOffline.IsChecked == true)
                {
                    this.Dispatcher.Invoke(() => {
                        col = offlineCollection;
                        mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
                    });
                }
                if (radioBtnBothStats.IsChecked == true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        col = fullCollection;
                        mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
                    });
                }
                else
                {
                    col = fullCollection;
                    mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
                }
                
                //После загрузки - выключаем видимость прогресс бара
                VisibleProgressOff();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnRefresh.IsEnabled = true;
        }

        //Прогружает новый список онлайн
        private void Refresh()
        {
            Task.Run(() =>
            {
                maker.HtmlString = htmlGetter.Refresh();
            });
            LoadAsync();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void radioBtnOnline_Checked(object sender, RoutedEventArgs e)
        {
            //Если выбран чек бокс - показывать только онлайн - показываем только онлайн и т.д.
            if (radioBtnBothStats.IsChecked == true)
                col = fullCollection;
            if (radioBtnOnline.IsChecked == true)
                col = onlineCollection;
            if (radioBtnOffline.IsChecked == true)
                col = offlineCollection;

            mM_MK_UnitDataGrid.ItemsSource = col.TheCollection;
        }
    }
}
