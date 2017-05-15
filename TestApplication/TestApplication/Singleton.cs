using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public class Singleton
    {
        private static Singleton instance;
        public string name;
        private static object lockObj = new object();
        private Singleton(string newName)
        {
            name = newName;
        }

        public static Singleton GetInstance(string name)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new Singleton(name);
                }
            }
            return instance;
        }
    }
}
