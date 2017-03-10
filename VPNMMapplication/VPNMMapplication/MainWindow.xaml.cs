using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace VPNMMapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string readyObjects = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string htmlText = File.ReadAllText(@"vpnmm\manage.htm", Encoding.UTF8);
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlText);
                HtmlNodeCollection collection = htmlDoc.DocumentNode.SelectNodes("//b");
                var doneCollection = from c in collection
                                     where (c.InnerText.StartsWith("МД") || c.InnerText.StartsWith("МК"))
                                     select c;
                if (doneCollection != null)
                {
                    foreach (HtmlNode node in doneCollection)
                    {
                        readyObjects += node.InnerText;
                        readyObjects += "\n";
                    }
                    txtAllText.Text = readyObjects;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            
        }

        public string getRequest(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                //httpWebRequest.Referer = "http://google.com"; // Реферер. Тут можно указать любой URL
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream,
                       Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
