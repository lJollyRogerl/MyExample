using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public abstract class Factory
    {
        public abstract Product MakeProduct();
    }

    public class IceCreamFactory: Factory
    {
        public override Product MakeProduct()
        {
            return new IceCream();
        }
    }
    public class MarshMallowFactory : Factory
    {
        public override Product MakeProduct()
        {
            return new MarshMallow();
        }
    }


    public abstract class Product
    {
    }

    public class IceCream: Product
    {
        public IceCream()
        {
            Console.WriteLine("Мороженное готово!");
        }
    }

    public class MarshMallow : Product
    {
        public MarshMallow()
        {
            Console.WriteLine("Зефир готово!");
        }
    }
}
