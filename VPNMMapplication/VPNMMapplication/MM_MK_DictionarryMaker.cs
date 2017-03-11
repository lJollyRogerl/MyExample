using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VPNMMapplication
{
    public class MM_MK_DictionarryMaker
    {
        //Строка с html содержимым
        public string HtmlString { get; set; }
        //Коллекция название ММ/DNS-имя
        public Dictionary<string, string> MM_MK_Dictionary { get; set; } = new Dictionary<string, string>();
        //Прогресс загрузки и наполнения коллекции
        public ProgressInfo ProgressOfLoading { get; set; } = new ProgressInfo();
        //Конструктор для загрузки из файла на локальной машине
        public MM_MK_DictionarryMaker(string fileAddress, Encoding fileEncoding)
        {
            try
            {
                string htmlText = File.ReadAllText(fileAddress, fileEncoding);
                HtmlString = htmlText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HtmlString = String.Empty;
            }
        }

        //Конструктор для загрузки по URL
        public MM_MK_DictionarryMaker(string htmlURL)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(htmlURL);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                //httpWebRequest.Referer = "http://google.com"; // Реферер. Тут можно указать любой URL
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                        {
                            HtmlString = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                HtmlString = String.Empty;
            }
        }

        //Загружаем небходимые имена и DNS из HTML, а так же создаем словарь.
        public async Task LoadDictionaryAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    MM_MK_Dictionary.Clear();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HtmlString);

                    //Выбираем все теги <b> на странице и из них те, что начинаются на МД||МК
                    var collectionOfNames = from c in htmlDoc.DocumentNode.SelectNodes("//b")
                                            where (c.InnerText.StartsWith("МД") || c.InnerText.StartsWith("МК"))
                                            select c;

                    var collectionOfDNS = from c in htmlDoc.DocumentNode.SelectNodes("/html/body/table/tbody/tr/*")
                                          where (c.InnerText.Trim().StartsWith("omd") || c.InnerText.Trim().StartsWith("omk"))
                                          select c;

                    //Т.К. в списке по 2 одинаковых значения, первому (который резервный) добавляем в имя соответсвующую отметку
                    for (int i = 0; i < collectionOfNames.Count(); i++)
                    {
                        if (i % 2 == 0 || i == 0)
                            MM_MK_Dictionary.Add(collectionOfNames.ElementAt(i).InnerText + " резерв",
                                collectionOfDNS.ElementAt(i).InnerText.Trim() + ".onlinemm.corp.tander.ru");
                        else
                            MM_MK_Dictionary.Add(collectionOfNames.ElementAt(i).InnerText,
                                collectionOfDNS.ElementAt(i).InnerText.Trim() + ".onlinemm.corp.tander.ru");

                        //Изменяем значение текущего прогресса и уведомляем об этом пользователя
                        ProgressOfLoading.TotalSteps = collectionOfNames.Count();
                        ProgressOfLoading.CurrentStep = i;
                        ProgressOfLoading.CurrentMM_MK = collectionOfNames.ElementAt(i).InnerText;
                        OnProgressChanged(ProgressOfLoading);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        //Событие, указывающее на изменение статуса прогресса загрузки
        public event Action<ProgressInfo> OnProgressChanged;
    }
}
