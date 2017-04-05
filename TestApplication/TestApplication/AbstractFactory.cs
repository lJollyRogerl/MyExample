using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public abstract class Teeth
    {
        public abstract string HowItLooks { get; }
        public abstract int Count { get; }
    }

    public class RabitTeeth : Teeth
    {
        public override int Count
        {
            get
            {
                return 22;
            }
        }

        public override string HowItLooks
        {
            get
            {
                return "Они маленькие и ровные! Два передних чуть побольше.";
            }
        }
    }

    public class TigerTeeth : Teeth
    {
        public override int Count
        {
            get
            {
                return 30;
            }
        }

        public override string HowItLooks
        {
            get
            {
                return "Они острые и с клыками!";
            }
        }
    }

    public abstract class DefendAbility
    {
        public abstract void Defend();
    }
    public class Run : DefendAbility
    {
        public override void Defend()
        {
            Console.WriteLine("Сбежать");
        }
    }
    public class Bite : DefendAbility
    {
        public override void Defend()
        {
            Console.WriteLine("Укусить");
        }
    }
    public abstract class EatHabbit
    {
        public abstract void Eat();
    }
    public class EatMeat : EatHabbit
    {
        public override void Eat()
        {
            Console.WriteLine("Есть мясо");
        }
    }
    public class EatGrass : EatHabbit
    {
        public override void Eat()
        {
            Console.WriteLine("Есть траву");
        }
    }
    public abstract class AnimalFactory
    {
        public abstract DefendAbility DefendCreator();
        public abstract EatHabbit EatHabbitCreator();
        public abstract Teeth TeethCreator();
    }
    public class RabbitFactory : AnimalFactory
    {
        public override DefendAbility DefendCreator()
        {
            return new Run();
        }

        public override EatHabbit EatHabbitCreator()
        {
            return new EatGrass();
        }

        public override Teeth TeethCreator()
        {
            return new RabitTeeth();
        }
    }
    public class TigerFactory : AnimalFactory
    {
        public override DefendAbility DefendCreator()
        {
            return new Bite();
        }

        public override EatHabbit EatHabbitCreator()
        {
            return new EatMeat();
        }

        public override Teeth TeethCreator()
        {
            return new TigerTeeth();
        }
    }

    public class Animal
    {
        public DefendAbility defendAbility;

        public EatHabbit eatHabbit;

        public Teeth teeth;

        public Animal(AnimalFactory factory)
        {
            defendAbility = factory.DefendCreator();
            eatHabbit = factory.EatHabbitCreator();
            teeth = factory.TeethCreator();
        }

        public void SelfDefending()
        {
            defendAbility.Defend();
        }
        public void Eating()
        {
            eatHabbit.Eat();
        }
        public void SayAboutItTeeth()
        {
            Console.WriteLine($"У него {teeth.Count} зубика и они {teeth.HowItLooks}");
        }
    }
}
