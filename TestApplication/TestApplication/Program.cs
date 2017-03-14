using System;
using System.IO;
using System.Net;
using System.Text;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {


            HttpWebResponse result = Post();
            CheckAutorisation(result);
            //string site = "http://www.professorweb.ru";

            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            //using (StreamReader stream = new StreamReader(
            //     resp.GetResponseStream(), Encoding.UTF8))
            //{
            //    System.Windows.Forms.MessageBox.Show(stream.ReadToEnd());
            //}
            //login_password
            //login_username
        }

        private static void CheckAutorisation(HttpWebResponse result)
        {
            try
            {
                if(result == null)
                    Console.WriteLine("result == null :(");
                string[] cookieVal = null;
                if (result.Headers["Set-Cookie"] != null)
                    cookieVal = result.Headers["Set-Cookie"].Split(new char[] { ',' });

                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                string answer = sr.ReadToEnd();
                sr.Close();
                result.Close();
                Console.WriteLine(answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private static HttpWebResponse Post()
        {
            try
            {
                HttpWebResponse result = null;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(@"https://www.avito.ru/profile/login?next=%2Fprofile");
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                byte[] SomeBytes = null;
                string FormParams = "login=ya.dmitrievgleb@yandex.ru&password=hbllbe4532&action=submit";
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
