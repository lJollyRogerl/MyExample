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
        private const string URL = @"https://vpnmm.corp.tander.ru/ovpn/login.cgi";

        //Очень важная часть, т.к. в этих куках будет храниться подтверждение удачно авторизации
        private CookieContainer cookies = new CookieContainer();

        public HTMLWithAutorization(string login, string password)
        {
            Login = login;
            PSWRD = password;
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

                //string[] cookieVal = null;
                //if (result.Headers["Set-Cookie"] != null)
                //    cookieVal = result.Headers["Set-Cookie"].Split(new char[] { ',' });

                Stream ReceiveStream = response.GetResponseStream();
                Encoding encode = Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                //То что нам вернул сервер не попытку авторизации
                string answer = sr.ReadToEnd();
                sr.Close();
                ReceiveStream.Close();
                response.Close();

                if (answer == "плохой ответ")
                {
                    System.Windows.MessageBox.Show("Авторизация неудалась.", "Ошибка!");
                    return null; 
                }
                else
                {
                    //CookieContainer cookie = new CookieContainer();
                    //foreach (string cook in cookieVal)
                    //{
                    //    string[] cookieArray = cook.Split(new char[] { ';' });
                    //    if (cookieArray.Length < 2)
                    //        continue;
                    //    cookie.Add(new Cookie(cookieArray[0].Split(new char[] { '=' })[0], cookieArray[0].Split(new char[] { '=' })[1],
                    //cookieArray[1].Split(new char[] { '=' })[1], cookieArray.Length > 2 ? cookieArray[2].Split(new char[] { '=' })[1] : ""));
                    //}

                    HttpWebRequest req1 = (HttpWebRequest)HttpWebRequest.Create(URL);
                    req1.UserAgent = "Mozilla/4.0+(compatible;+MSIE+5.01;+Windows+NT+5.0)";
                    //Вот оно - важное дополнение.
                    req1.CookieContainer = cookies;
                    req1.Method = "GET";
                    HttpWebResponse result1 = (HttpWebResponse)req1.GetResponse();
                    Stream ReceiveStream1 = result1.GetResponseStream();
                    StreamReader sr1 = new StreamReader(ReceiveStream1, encode);
                    string html = sr1.ReadToEnd();
                    result1.Close();
                    return html;
                }


            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка!");
                    return null;
            }

        }

        private HttpWebResponse Post(string login, string password)
        {
            try
            {
                HttpWebResponse result = null;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(URL);
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
