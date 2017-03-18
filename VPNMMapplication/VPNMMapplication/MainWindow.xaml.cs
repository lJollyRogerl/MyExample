using System;
using System.Text;
using System.Windows;

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
        public MainWindow(MM_MK_DictionarryMaker dictionaryMaker)
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
            txtAllText.Text = maker.HtmlString;
            //LoadAsync();
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
                await maker.LoadDictionaryAsync();
                //После загрузки - выключаем видимость прогресс бара
                VisibleProgressOff();
                //Выводим на текстбокс всю выборку имен ММ/МК
                foreach (var mm_mk in maker.MM_MK_Dictionary)
                {
                    readyObjects += mm_mk.Key + " - " + mm_mk.Value;
                    readyObjects += "\n";
                }
                txtAllText.Text = readyObjects;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btnLoad.IsEnabled = true;
        }
    }
}
