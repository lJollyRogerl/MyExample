﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;

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
        MM_MK_DictionarryMaker maker;
        //основное окно. Создается после выбора загрузки
        MainWindow mainWindow;
        //Ссылка на страницу, с которой будет производиться загрузка
        AddingNewFilialWindow adFilWin;
        //Текущий выбранный в комбо боксе филиал
        Filial currentFilial;
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
            if (radioHttpLoad.IsChecked == true)
            {
                stackPanelLogin.IsEnabled = true;
                stackPanelPassword.IsEnabled = true;
                stackPanelComboBox.IsEnabled = true;
                stackPanelOfflineLoad.IsEnabled = false;
                gbLoadWithHttpRequest.Header = "Необходимо пройти доменную авторизацию";
                gbLoadWithLocalHtmlPage.Header = "";
            }

            if (radioHttpPage.IsChecked == true)
            {
                stackPanelLogin.IsEnabled = false;
                stackPanelPassword.IsEnabled = false;
                stackPanelComboBox.IsEnabled = false;
                stackPanelOfflineLoad.IsEnabled = true;
                gbLoadWithHttpRequest.Header = "";
                gbLoadWithLocalHtmlPage.Header = "Необходимо загрузить сохраненный *.html";
            }
        }

        private void btnPickFile_Click(object sender, RoutedEventArgs e)
        {
            RunFileDialog();
        }

        private void RunFileDialog()
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
                openFile.Filter = "html страница (*.html, *.htm) | *.html; *.htm";
                if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = openFile.FileName;
                    txtPathToHtmlFile.Text = openFile.SafeFileName;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationAsync();
        }
        private void LoadFilials()
        {
            //SerializeDivisions.AddFillial(divisions, new Filial("Нижне-Тагильский", new Region("Урал-Западный")));
            //SerializeDivisions.AddFillial(divisions, new Filial("Пермский", new Region("Урал-Западный")));
            //SerializeDivisions.AddFillial(divisions, new Filial("Серовский", new Region("Урал-Западный")));
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

        private async void AuthorizationAsync()
        {
            try
            {
                btnLoad.IsEnabled = false;
                await DoAuthorisationAsync();
                btnLoad.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        //Попытка авторизоваться и при удачной авторизации выгрузить html разметку
        public async Task DoAuthorisationAsync()
        {
            await Task.Run(() =>
            {
                try
                {

                    //Если загрузка идет через HttpWebRequest
                    if (radioHttpLoad.IsChecked == true)
                    {
                        HTMLWithAutorization htmlGetter = new HTMLWithAutorization(txtLogin.Text, pbPassword.Password,
                            currentFilial);

                        if (htmlGetter.HTML == "Неверный логин или пароль!")
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show("Неверный логин или пароль!", "Ошибка!");
                            });
                            return;
                        }
                        maker = new MM_MK_DictionarryMaker(htmlGetter.HTML);
                        mainWindow = new MainWindow(maker);
                        mainWindow.Show();
                        this.Close();
                    }

                    //Если загруженна локальная страница
                    if (radioHttpPage.IsChecked == true)
                    {
                        string htmlText = File.ReadAllText(pathToFile, Encoding.UTF8);
                        maker = new MM_MK_DictionarryMaker(htmlText);
                        mainWindow = new MainWindow(maker);
                        mainWindow.Show();
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!");
                }
            });

            
        }
    }
}
