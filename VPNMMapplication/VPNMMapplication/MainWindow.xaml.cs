﻿using System;
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
        string readyObjects = "";
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
                MM_MK_Collection col = await maker.LoadCollectionAsync(false);
                //После загрузки - выключаем видимость прогресс бара
                VisibleProgressOff();
                foreach (var item in col.TheCollection)
                {
                    readyObjects += $"Название - {item.Title}, IP - {item.IP}, Подключен? - {item.IsOnline}, DNS - {item.DNS_Name}";
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
