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
        public List<string> MM_MK_List = new List<string>();
        //Прогресс загрузки и наполнения коллекции
        public ProgressInfo ProgressOfLoading { get; set; } = new ProgressInfo();
        //Конструктор для загрузки из файла на локальной машине
        public MM_MK_DictionarryMaker(string htmlText)
        {
                HtmlString = htmlText;
        }

        public async Task LoadListAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    MM_MK_List.Clear();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HtmlString);
                    var collectionOfDNS = from c in htmlDoc.DocumentNode.SelectNodes("/html/body/table/tbody/tr/td.data2/*")
                                          select c;
                    foreach (var item in collectionOfDNS)
                    {
                        MM_MK_List.Add(item.InnerText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "in loadListAsync");
                }
            });
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
