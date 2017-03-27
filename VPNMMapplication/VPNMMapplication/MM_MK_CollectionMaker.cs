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

        public HTMLWithAutorization htmlMaker { get; set; }
        //Конструктор для загрузки из файла на локальной машине
        public MM_MK_CollectionMaker(string htmlText, HTMLWithAutorization htmMaker)
        {
            HtmlString = htmlText;
            htmlMaker = htmMaker;
        }

        //Собирает кллекцию из статуса подключения. true - есть подключение. false - нет.
        public async Task<MM_MK_Collection> LoadCollectionAsync(bool isConnected, bool? doDateLogLoad)
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
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HtmlString);
                    MM_MK_Collection unitCollection = new MM_MK_Collection();

                    var collectionOfNodes = from c in htmlDoc.DocumentNode.SelectNodes("/html/body/table/tbody/tr")
                                            where c.InnerHtml.Contains(status)
                                            select c;

                    ProgressOfLoading.TotalSteps = collectionOfNodes.Count();

                    for (int i = 0; i < collectionOfNodes.Count(); i++)
                    {
                        var node = collectionOfNodes.ElementAt(i);
                        MM_MK_Unit addingUnit = new MM_MK_Unit();
                        //foreach (var child in node.ChildNodes)
                        //{
                        //    //Если подузел содержит МД, МК или ТЦ - добавляем в имя
                        //    if (child.InnerText.Contains("МД") || child.InnerText.Contains("МК") ||
                        //        child.InnerText.Contains("ТЦ") || child.InnerText.Contains("Микроофис"))
                        //        addingUnit.Title = child.InnerText.Trim();

                        //    //Если подузел содержит omd, omk или otc - добавляем в DNS имя
                        //    if (child.InnerText.Contains("omd") || child.InnerText.Contains("omk") ||
                        //        child.InnerText.Contains("otc") || child.InnerText.Contains("omf"))
                        //    {
                        //        string objName = child.InnerText.Trim();
                        //        if(isConnected == false && doDateLogLoad == true)
                        //            addingUnit.LastDateOnline = GetLastSessionDate(objName);
                        //        addingUnit.DNS_Name = objName + ".onlinemm.corp.tander.ru";
                        //    }

                        //    //Если есть строка, начинающаяся с 10 - добавляем в ip
                        //    if (child.InnerText.Trim().StartsWith("10.") || (child.InnerText.Trim().StartsWith("172.")))
                        //        addingUnit.IP = child.InnerText.Trim();

                        //    //Если днс имя присвоено - проверяем
                        //    if (addingUnit.DNS_Name != null)
                        //    {
                        //        if (addingUnit.DNS_Name.Contains("_1"))
                        //            addingUnit.MainOrReserve = "Резервный";
                        //        else
                        //            addingUnit.MainOrReserve = "Основной";
                        //    }
                        //    //устанавливаю статус подключения
                        //    addingUnit.IsOnline = isConnected;

                        //}

                        addingUnit.Title = node.ChildNodes.ElementAt(4).InnerText.Trim();
                        string objName = node.ChildNodes.ElementAt(8).InnerText.Trim();
                        if (isConnected == false && doDateLogLoad == true)
                            addingUnit.LastDateOnline = GetLastSessionDate(objName);
                        addingUnit.DNS_Name = objName + ".onlinemm.corp.tander.ru";
                        addingUnit.IP = node.ChildNodes.ElementAt(12).InnerText.Trim();
                        if (addingUnit.DNS_Name != null)
                        {
                            if (addingUnit.DNS_Name.Contains("_1"))
                                addingUnit.MainOrReserve = "Резервный";
                            else
                                addingUnit.MainOrReserve = "Основной";
                        }
                        //устанавливаю статус подключения
                        addingUnit.IsOnline = isConnected;
                        unitCollection.Add(addingUnit);
                        ProgressOfLoading.CurrentStep = i;
                        ProgressOfLoading.CurrentMM_MK = addingUnit.Title;
                        OnProgressChanged(ProgressOfLoading);
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

        //Выбирает дату последнего подключения
        public string GetLastSessionDate(string objectName)
        {
            try
            {
                string result = "";
                HtmlDocument htmlDoc = new HtmlDocument();
                string html = htmlMaker.GetSessionsLog(objectName);
                htmlDoc.LoadHtml(html);
                var collectionOfNodes = from c in htmlDoc.DocumentNode.SelectNodes("/html/body/table/tbody/tr/*")
                                        where c.ChildNodes.Count < 2
                                        select c;

                result = collectionOfNodes.ElementAt(collectionOfNodes.Count() - 7).InnerText.Trim();
                if (string.IsNullOrWhiteSpace(result))
                    result = "Нет данных";                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
                return null;
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
