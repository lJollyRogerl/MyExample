﻿using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        string pathToFile;
        public SplashScreen()
        {
            InitializeComponent();
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
                stackPanelOfflineLoad.IsEnabled = false;
                gbLoadWithHttpRequest.Header = "Необходимо пройти доменную авторизацию";
                gbLoadWithLocalHtmlPage.Header = "";
            }

            if (radioHttpPage.IsChecked == true)
            {
                stackPanelLogin.IsEnabled = false;
                stackPanelPassword.IsEnabled = false;
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
                openFile.Filter = "html страница (*.html) | *.html";
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
    }
}