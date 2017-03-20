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
    public class MM_MK_CollectionMaker
    {
        //Строка с html содержимым
        public string HtmlString { get; set; }
        //Коллекция название ММ/DNS-имя
        public Dictionary<string, string> MM_MK_Dictionary { get; set; } = new Dictionary<string, string>();
        public List<string> MM_MK_List = new List<string>();
        //Прогресс загрузки и наполнения коллекции
        public ProgressInfo ProgressOfLoading { get; set; } = new ProgressInfo();
        //Конструктор для загрузки из файла на локальной машине
        public MM_MK_CollectionMaker(string htmlText)
        {
                HtmlString = htmlText;
        }

        //Собирает кллекцию из статуса подключения. true - есть подключение. false - нет.
        public async Task<MM_MK_Collection> LoadCollectionAsync(bool isConnected)
        {
            string status;
            if (isConnected == false)
                status = "Нет подключения";
            else
                status = "Есть подключение";
            return await Task.Run(() =>
            {
                try
                {
                    MM_MK_List.Clear();
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HtmlString);
                    MM_MK_Collection unitCollection = new MM_MK_Collection();

                    var collectionOfNames = from c in htmlDoc.DocumentNode.SelectNodes("/html/body/table/tbody/tr")
                                            where c.InnerHtml.Contains(status)
                                            select c;

                    foreach (var node in collectionOfNames)
                    {
                        MM_MK_Unit addingUnit = new MM_MK_Unit();
                        foreach (var child in node.ChildNodes)
                        {
                            //Если подузел содержит МД, МК или ТЦ - добавляем в имя
                            if (child.InnerText.Contains("МД") || child.InnerText.Contains("МК") ||
                                child.InnerText.Contains("ТЦ"))
                                addingUnit.Title = child.InnerText.Trim();

                            //Если подузел содержит omd, omk или otc - добавляем в DNS имя
                            if (child.InnerText.Contains("omd") || child.InnerText.Contains("omk") || 
                                child.InnerText.Contains("otc"))
                                addingUnit.DNS_Name = child.InnerText.Trim() + ".onlinemm.corp.tander.ru";
                        }
                        unitCollection.Add(addingUnit);
                    }
                    return unitCollection;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка выборки!");
                    return null;
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
