using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace VPNMMapplication
{
    public partial class MainWindow : Window
    {
        private static bool isTableSessionLoaded = false;
        private void menuNewSession_Click(object sender, RoutedEventArgs e)
        {
            InvokeNewSession();
        }

        private void InvokeNewSession()
        {
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            this.Close();
        }

        private void menuToExel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = "exportToExcel.сsv";
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                saveFile.Filter = "Таблица (*.csv) | *.csv";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = saveFile.FileName;
                    DataGrid dg = mM_MK_UnitDataGrid;
                    dg.SelectAllCells();
                    dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                    ApplicationCommands.Copy.Execute(null, dg);
                    dg.UnselectAllCells();
                    String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    StreamWriter swObj = new StreamWriter(pathToFile);
                    swObj.WriteLine(Clipboardresult);
                    swObj.Close();
                    Process.Start(pathToFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void menuToXmlForRMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathToFile = "ImportToRMS.xml";
                System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog();
                saveFile.Filter = "Файл импорта (*.xml) | *.xml";
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pathToFile = saveFile.FileName;
                    SaveToXMLForRMS.BuildXmlDoc(fullCollection, pathToFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void helpDesсription_Click(object sender, RoutedEventArgs e)
        {
            //Написать код, который будет выводить окно с подсказками
            MessageBox.Show("Описание не написал, т.к. недавно пришло письмо от Марата по поводу того, что VPNMM будут заменять на другой ресурс. Нет смылса дальше развивать это приложение.", "Тут должно быть описание");
        }

        private void helpWriteToDeveloper_Click(object sender, RoutedEventArgs e)
        {
            string subject = "Предложения и пожелания VPNMMapplication";
            Process.Start("mailto:dmitriev_gv@ntagil.magnit.ru" + "?subject=" + subject);
        }

        //выбор таблицы для вывода на экран
        private void comboWhatToShow_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if ((string)comboWhatToShow.SelectedValue == "Мониторинг")
                {
                    stackSessionScrollViewer.Visibility = Visibility.Collapsed;
                    mM_MK_UnitDataGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    //Если таблица уже загружена - просто отобразить. Если нет - загрузить
                    if (isTableSessionLoaded == false)
                    {
                        LoadSessionTable();
                        //SessionsLog.OnSessionAded += SessionsLog_OnSessionAded;
                    }
                    stackSessionScrollViewer.Visibility = Visibility.Visible;
                    mM_MK_UnitDataGrid.Visibility = Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //Загружает таблицу логов в первый раз
        private void LoadSessionTable()
        {
            try
            {
                isTableSessionLoaded = true;
                if(SessionsLog == null)
                    SessionsLog = new SessionsArray(fullCollection);
                for (int i = 0; i < SessionsLog.Sessions.Count; i++)
                {
                    AddNewSessionAtTable(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source);
            }
        }

        //Добавляет новый список в таблицу логов
        private void AddNewSessionAtTable(int index)
        {
            if (stackSessionsView.Children.Count > 4)
                stackSessionsView.Children.RemoveAt(0);
            DataGrid dgLstViewSessions = new DataGrid();
            DataGridTextColumn column = new DataGridTextColumn();
            column.Header = SessionsLog.Sessions[index].Statuses[0].TheDate.ToString("dd/MM/yyyy HH:mm");
            column.Binding = new Binding("TitleAndState");
            dgLstViewSessions.Columns.Add(column);

            foreach (var item in SessionsLog.Sessions[index].Statuses)
            {
                dgLstViewSessions.Items.Add(item);
            }
            stackSessionsView.Children.Add(dgLstViewSessions);
            dgLstViewSessions.LoadingRow += LstViewSessions_LoadingRow;
        }

        private void LstViewSessions_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //Ели в предыдущей сесси данный объект не был в сети, то окрашиваем в красный
            if (e.Row.DataContext is PreviousSessionState)
            {
                PreviousSessionState state = (PreviousSessionState)e.Row.DataContext;
                if (state.TitleAndState.Contains("Не"))
                {
                    e.Row.Background = new SolidColorBrush(Color.FromRgb(245, 0, 41));
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
            }
        }

        //Создает и открывает окно настроек
        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(settings == null)
                    settings = new Settings() { TimePerLogging = 4 };
                
                settingsWindow = new SettingsWindow(settings);
                settingsWindow.Owner = this;
                if (settingsWindow.ShowDialog() == true)
                {
                    settings = settingsWindow.settings;
                    logSerializationTimer.Stop();
                    logSerializationTimer.Start();
                    
                    logSerializationTimer.Interval = new TimeSpan(settings.TimePerLogging, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка открытия окна настроек");
            }
           
        }
        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            comboWhatToShow.SelectedIndex = 0;
        }
    }
}
