using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private MM_MK_Collection currentDisplayedCol = new MM_MK_Collection();
        private bool? isOnlineMode = false;
        private bool firstLoad = true;
        MM_MK_Collection onlineCollection = new MM_MK_Collection();
        MM_MK_Collection offlineCollection = new MM_MK_Collection();
        MM_MK_Collection fullCollection = new MM_MK_Collection();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer logSerializationTimer = new DispatcherTimer();
        SessionsArray SessionsLog;
        Settings settings = new Settings();
        SettingsWindow settingsWindow;

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MM_MK_CollectionMaker dictionaryMaker)
        {
            maker = dictionaryMaker;
            InitializeComponent();
        }

        public MainWindow(MM_MK_CollectionMaker dictionaryMaker, HTMLWithAutorization getterForRefresh)
        {
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
            radioBtnBothStats.IsChecked = true;
            radioBtnOnline.Checked += radioBtnOnline_Checked;
            radioBtnOffline.Checked += radioBtnOnline_Checked;
            radioBtnBothStats.Checked += radioBtnOnline_Checked;
            maker.OnProgressChanged += Maker_OnProgressChanged;
            htmlGetter.OnAuthorizationProgress += HtmlGetter_OnAuthorizationProgress;
            mM_MK_UnitDataGrid.LoadingRow += MM_MK_UnitDataGrid_LoadingRow;
            checkBoxShowDate.ToolTip = "При выборе данной опции загрузка будет проходить намного дольше.\n" +
                "Это происходит из за того, что программа переходит по ссылке в историю подклюений и выбирает\n" +
                "последнюю сессию из списка для каждого магазина.";

            LoadAsync();
            firstLoad = false;
            //обновляет статус ММ каждые 7 минут
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += delegate (object s, EventArgs eArgs) { LoadAsync(); };
            dispatcherTimer.Interval = new TimeSpan(0, 7, 0);
            dispatcherTimer.Start();

            //записывает лог каджые 4 часа
            logSerializationTimer = new DispatcherTimer();
            logSerializationTimer.Tick += delegate (object s, EventArgs eArgs) 
            {
                if (SessionsLog == null)
                {
                    SessionsLog = new SessionsArray(fullCollection);
                }
                else
                    SessionsLog.Add(fullCollection);
            };
            logSerializationTimer.Interval = new TimeSpan(0, 0, 20);
            logSerializationTimer.Start();
        }

        private void HtmlGetter_OnAuthorizationProgress(string obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Hidden;
                lblStatus.Content = obj;
            });
        }

        private void Maker_OnProgressChanged(ProgressInfo obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Visible;
                progressBar.Maximum = obj.TotalSteps;
                progressBar.Value = obj.CurrentStep;
                lblStatus.Content = "Обработка " + obj.CurrentMM_MK.ToString();
            });
        }

        public void VisibleProgressOn()
        {
            statusBar.Visibility = Visibility.Visible;
        }

        public void VisibleProgressOff()
        {
            statusBar.Visibility = Visibility.Collapsed;
        }

        private async void LoadAsync()
        {
            try
            {
                //Перед нечалом загрузки - включаем видимость прогресс бара
                //И отключаем кнопку "обновить"
                VisibleProgressOn();
                btnRefresh.IsEnabled = false;
                //Если загрузка производится не в первый раз - обновить html строку
                if (firstLoad == false)
                {
                    maker.HtmlString = "";
                    maker.HtmlString = await htmlGetter.Refresh();
                }
                dispatcherTimer.Stop();
                dispatcherTimer.Start();
                //Коллекция онлайн объектов
                onlineCollection = await maker.LoadCollectionAsync(true, checkBoxShowDate.IsChecked);
                onlineCollection.TheCollection.Sort(new MMCollectionComparer());
                //Коллекция оффлайн объектов
                offlineCollection = await maker.LoadCollectionAsync(false, checkBoxShowDate.IsChecked);
                offlineCollection.TheCollection.Sort(new MMCollectionComparer());
                //Коллекция онлайн+оффлайн объектов
                fullCollection = new MM_MK_Collection();
                fullCollection.AddCollection(onlineCollection);
                fullCollection.AddCollection(offlineCollection);
                fullCollection.TheCollection.Sort(new MMCollectionComparer());
                //После загрузки - выключаем видимость прогресс бара
                VisibleProgressOff();
                //Выбор текущей колекции
                SwitchView();
                btnRefresh.IsEnabled = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                InvokeNewSession();
            }
        }


        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
             LoadAsync();
        }

        private void radioBtnOnline_Checked(object sender, RoutedEventArgs e)
        {
            //Если выбран чек бокс - показывать только онлайн - показываем только онлайн и т.д.
            SwitchView();
        }

        private void SwitchView()
        {
            //Если выбранно показывать онлайн коллекцию - показываем ее, оффлайн - показываем её, 
            //в остальных случаях показываем полную коллекцию
            if (radioBtnOnline.IsChecked == true)
                currentDisplayedCol = onlineCollection;

            else if (radioBtnOffline.IsChecked == true)
                currentDisplayedCol = offlineCollection;

            else
                currentDisplayedCol = fullCollection;

            mM_MK_UnitDataGrid.ItemsSource = currentDisplayedCol.TheCollection;
            lblCurrentCount.Content = currentDisplayedCol.TheCollection.Count;
        }

        private void sliderFrequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtFrequency.Text = $"Частота обновления {(int)e.NewValue} минут";
            dispatcherTimer.Interval = TimeSpan.FromMinutes((int)e.NewValue);
        }


    }
}
