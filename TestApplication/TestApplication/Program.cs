using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractFactoryFunc();
        }

        private static void AbstractFactoryFunc()
        {
            Animal animal = null;
            for (;;)
            {
                Console.WriteLine("Выберите животного: [1] - Тигр, [2] - Заяц, [0] - Выход");
                switch (Console.ReadLine())
                {
                    case "0":
                        return;
                    case "1":
                        animal = new Animal(new TigerFactory());
                        break;
                    case "2":
                        animal = new Animal(new RabbitFactory());
                        break;
                    default:
                        continue;
                }
                Console.WriteLine("Наше животное в случае опасности может:");
                animal.SelfDefending();
                Console.WriteLine("А если проголодается, то животное будет:");
                animal.Eating();
                animal.SayAboutItTeeth();
            }
        }

        private static void FactoryMethodFunction()
        {
            Factory factory = null;
            for (;;)
            {

                Console.WriteLine("Выберите фабрику: [1] - Мороженного, [2] - Зефира, [0] - Выход");
                switch (Console.ReadLine())
                {
                    case "0":
                        return;
                    case "1":
                        factory = new IceCreamFactory();
                        break;
                    case "2":
                        factory = new MarshMallowFactory();
                        break;
                    default:
                        return;
                }
                var product = factory.MakeProduct();
            }
        }

        

        private static void FuncTwo()
        {
            // Creates an HttpWebRequest for the specified URL. 
            Uri uri = new Uri(@"https://vk.com");
            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            myHttpWebRequest.CookieContainer = cookieContainer;
            // Sends the HttpWebRequest and waits for response.
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            // Displays all the headers present in the response received from the URI.
            Console.WriteLine("\r\nSet-cookie header: ");
            // Displays each header and it's key associated with the response.
            //for (int i = 0; i < myHttpWebResponse.Headers.Count; ++i)
            //    Console.WriteLine("\nHeader Name:{0}, Value :{1}", myHttpWebResponse.Headers.Keys[i], myHttpWebResponse.Headers[i]);
            Console.WriteLine(myHttpWebResponse.Headers["Set-Cookie"]);
            try
            {
                Console.WriteLine("\nNow we'll be trying to sepatrete each cook from those set.\n");
                foreach (Cookie cook in cookieContainer.GetCookies(uri))
                {
                    Console.WriteLine($"Name = {cook.Name}, Value = {cook.Value}");
                }
                //string[] cookieVal = null;
                //if (myHttpWebResponse.Headers["Set-Cookie"] != null)
                //    cookieVal = myHttpWebResponse.Headers["Set-Cookie"].Split(new char[] { ';' });

                //Console.WriteLine("There is cookie values:");
                //Dictionary<string, string> dic = new Dictionary<string, string>();
                //foreach (var item in cookieVal)
                //{
                //    string[] str = item.Split('=');
                //    dic.Add(str[0], str[1]);
                //}
                //foreach (var dicUnit in dic)
                //{
                //    Console.WriteLine($"Key = {dicUnit.Key}, value = {dicUnit.Value}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            // Releases the resources of the response.
            myHttpWebResponse.Close();
        }

        private static async void SomeFunc()
        {
            
            string answer = await getString();
            Console.WriteLine("done");
            Console.WriteLine(answer);
        }

        private static async Task<string> getString()
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            NetworkCredential credentials = new NetworkCredential(@"ya.dmitrievgleb@yandex.ru", @"hbllbe271090");
            handler.Credentials = credentials;
            handler.CookieContainer = cookies;
            HttpClient client = new HttpClient(handler);
            Uri site = new Uri(@"https://www.italki.com");
            Stream myStream = await client.GetStreamAsync(site);
            var byteArray = new byte[myStream.Length];
            myStream.Read(byteArray, 0, byteArray.Length);
            StreamReader sr = new StreamReader(myStream, Encoding.UTF8);
            string answer = sr.ReadToEnd();
            Console.WriteLine(answer);
            if(answer == null)
                Console.WriteLine("Answer is null");
            myStream.Close();
            sr.Close();
            return answer;
            //string str = ((StreamReader)myStream).ReadToEnd();
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
