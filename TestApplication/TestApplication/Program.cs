using System;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("He, I'm here!");
            Console.Beep();
            int i = int.Parse(Console.Read());
            string message = "";
            if(i>20)
                message = "Your int is bigger then 20";
            else
                message = "Your int is less then 20";
            Console.WriteLine(message);
        }
    }
}
