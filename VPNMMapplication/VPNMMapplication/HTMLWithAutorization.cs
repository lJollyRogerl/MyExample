using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VPNMMapplication
{
    class HTMLWithAutorization
    {
        public string Login { get; set; }
        public string PSWRD { get; set; }
        public string Filial { get; set; }
        private const string URL = @"https://vpnmm.corp.tander.ru/ovpn/";
        Encoding encode = Encoding.GetEncoding("utf-8");

        //Очень важная часть, т.к. в этих куках будет храниться подтверждение удачно авторизации
        private CookieContainer cookies = new CookieContainer();

        public HTMLWithAutorization(string login, string password, string filial)
        {
            Login = login;
            PSWRD = password;
            Filial = filial;
        }
        public string HTML
        {
            get
            {
                return GetHTML(Post(Login, PSWRD));
            }
        }

        private string GetHTML(HttpWebResponse response)
        {
            try
            {
                if (response == null)
                {
                    System.Windows.MessageBox.Show("Авторизация неудалась.", "Ошибка!");
                    return null;
                }
                Stream ReceiveStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                //То что нам вернул сервер не попытку авторизации
                string answer = sr.ReadToEnd();
                sr.Close();
                ReceiveStream.Close();
                response.Close();

                return DoRequestWithCookie("manage.cgi", "whitey");

                //if (answer == "плохой ответ")
                //{
                //    System.Windows.MessageBox.Show("Авторизация неудалась.", "Ошибка!");
                //    return null; 
                //}
                //else
                //{
                //    string FormParams = $"unrollr=Урал-Западный";
                //    DoRequestWithCookie(FormParams);
                //    FormParams = $"unrollf={Filial}";
                //    return DoRequestWithCookie(FormParams);
                //}


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка!");
                    return null;
            }

        }

        private string DoRequestWithCookie(string addToUrl, string nextLink)
        {
            string html = GetResponseInHTML(URL + addToUrl);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode.SelectSingleNode($"//a[@class='{nextLink}']");
            if (node != null)
            {
                return DoRequestWithCookie(node.Attributes["href"].Value, "blackey");
            }
            else
            {
                return html;
            }
        }

        private string GetResponseInHTML(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = "Mozilla/4.0+(compatible;+MSIE+5.01;+Windows+NT+5.0)";
            req.AllowAutoRedirect = false;
            req.ContentType = "application/x-www-form-urlencoded";
            //Вот оно - важное дополнение.
            req.CookieContainer = cookies;
            req.Method = "POST";
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream ReceiveStream1 = response.GetResponseStream();
            StreamReader sr1 = new StreamReader(ReceiveStream1, encode);
            return sr1.ReadToEnd();
        }

        private HttpWebResponse Post(string login, string password)
        {
            try
            {
                HttpWebResponse result = null;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL+ "login.cgi");
                //cookies - глобальная переменная
                req.CookieContainer = cookies;
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;
                string FormParams = $"log_username={login}&log_password={password}&action=login";
                SomeBytes = Encoding.UTF8.GetBytes(FormParams);
                req.ContentLength = SomeBytes.Length;
                Stream newStream = req.GetRequestStream();
                newStream.Write(SomeBytes, 0, SomeBytes.Length);
                newStream.Close();
                result = (HttpWebResponse)req.GetResponse();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
    }
}
