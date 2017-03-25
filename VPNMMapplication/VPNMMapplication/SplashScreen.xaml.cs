using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        //переменная для хранения данных о филлиалах и регионах. 
        //Сериализуется/десериализуется в ListOfFillials.xml
        private Divisions divisions = new Divisions();
        //путь к файлу для локальной загрузки
        string pathToFile;
        //Создатель коллекции название ММ - его DNS
        MM_MK_CollectionMaker maker;
        //основное окно. Создается после выбора загрузки
        MainWindow mainWindow;
        //Ссылка на страницу, с которой будет производиться загрузка
        AddingNewFilialWindow adFilWin;
        //Текущий выбранный в комбо боксе филиал
        Filial currentFilial;
        //Таймер ля оформления загрузки
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        HTMLWithAutorization htmlGetter;
        public SplashScreen()
        {
            InitializeComponent();
            divisions.DivisionLoad();
            LoadFilials();
        }

        

        private void radioHttpLoad_Checked(object sender, RoutedEventArgs e)
        {
            //Если выбрана загрузка по http - включаем поля авторизации.
            //Если выбрана локальная загрузка - включаем поля для загрузки из файла .http
            //Остальне поля - отключаем
            //if (radioHttpLoad.IsChecked == true)
            //{
                stackPanelLogin.IsEnabled = true;
                stackPanelPassword.IsEnabled = true;
                stackPanelComboBox.IsEnabled = true;
                //stackPanelOfflineLoad.IsEnabled = false;
                gbLoadWithHttpRequest.Header = "Необходимо пройти доменную авторизацию";
                //gbLoadWithLocalHtmlPage.Header = "";
            //}

            //if (radioHttpPage.IsChecked == true)
            //{
            //    stackPanelLogin.IsEnabled = false;
            //    stackPanelPassword.IsEnabled = false;
            //    stackPanelComboBox.IsEnabled = false;
            //    stackPanelOfflineLoad.IsEnabled = true;
            //    gbLoadWithHttpRequest.Header = "";
            //    gbLoadWithLocalHtmlPage.Header = "Необходимо загрузить сохраненный *.html";
            //}
        }

        //private void btnPickFile_Click(object sender, RoutedEventArgs e)
        //{
        //    RunFileDialog();
        //}

        //private void RunFileDialog()
        //{
        //    try
        //    {
        //        System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
        //        openFile.Filter = "html страница (*.html, *.htm) | *.html; *.htm";
        //        if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            pathToFile = openFile.FileName;
        //            txtPathToHtmlFile.Text = openFile.SafeFileName;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.MessageBox.Show(ex.Message);
        //    }
        //}

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            DoBtnLoadJob();

        }

        private async void DoBtnLoadJob()
        {
            statusGrid.Visibility = Visibility.Visible;
            btnLoad.IsEnabled = false;
            authorisStackPnl.IsEnabled = false;
            lblStatusBar.Content = "Пожалуйста, ожидайте.";
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
            await DoAsyncAuthorization();
            btnLoad.IsEnabled = true;
            authorisStackPnl.IsEnabled = true;
            statusGrid.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
            lblStatusBar.Content = "";
        }

        private void LoadFilials()
        {
            List<String> filialNames = divisions.GetAllFilialNamesAsList();
            filialNames.Add("<Добавить новый>");
            comboBoxChooseFilial.ItemsSource = filialNames;
            comboBoxChooseFilial.SelectedIndex = 0;
        }

        private void comboBoxChooseFilial_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((string)comboBoxChooseFilial.SelectedItem == "<Добавить новый>")
            {
                adFilWin = new AddingNewFilialWindow(divisions);
                adFilWin.Owner = this;
                if (adFilWin.ShowDialog() == true)
                    LoadFilials();
            }
            else
            {
                currentFilial = divisions.FindFilialByName(comboBoxChooseFilial.SelectedItem.ToString());
            }
        }

        private async Task DoAsyncAuthorization()
        {
            try
            {
                //Если загрузка идет через HttpWebRequest
                //if (radioHttpLoad.IsChecked == true)
                //{
                    if ((string.IsNullOrEmpty(txtLogin.Text)) || (string.IsNullOrEmpty(pbPassword.Password)))
                    {
                        MessageBox.Show("Заполните пожалуйста оба поля!", "Ошибка!");
                        return;
                    }
                    htmlGetter = new HTMLWithAutorization(txtLogin.Text, pbPassword.Password,
                        currentFilial);
                    htmlGetter.OnAuthorizationProgress += HtmlGetter_OnAuthorizationProgress;

                    string html = await Task.Run<string> (()=>
                    {
                        return htmlGetter.GetHTMLString();
                    });

                    if (html == null)
                    {
                        return;
                    }
                    if (html == "Неверный логин или пароль!")
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка!");
                        return;
                    }

                    maker = new MM_MK_CollectionMaker(html, htmlGetter);
                    mainWindow = new MainWindow(maker, htmlGetter, true/*radioHttpLoad.IsChecked*/);
                    mainWindow.Show();
                    this.Close();
                //}

                ////Если загруженна локальная страница
                //if (radioHttpPage.IsChecked == true)
                //{
                //    string htmlText = File.ReadAllText(pathToFile, Encoding.UTF8);
                //    maker = new MM_MK_CollectionMaker(htmlText, htmlGetter);
                //    mainWindow = new MainWindow(maker);
                //    mainWindow.Show();
                //    this.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }
        private void HtmlGetter_OnAuthorizationProgress(string obj)
        {
            this.Dispatcher.Invoke(() => {
                lblStatusBar.Content = obj;
            });
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            lblStatusBar.Content += ".";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //radioHttpLoad.IsChecked = true;
        }
    }
}
