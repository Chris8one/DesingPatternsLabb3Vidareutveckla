using System;
using System.Collections.Generic;

namespace DesingPatternsLabb3Vidareutveckla
{
    public interface IWarmDrink
    {
        void Consume();
    }
    
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }
    
    internal class Coffe : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot Coffe is served.");
        }
    }
    
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served.");
        }
    }
    
    internal class Chocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot chocolate is served.");
        }
    }
    
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }
    
    internal class CoffeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot coffe in your cup");
            
            return new Coffe();
        }
    }
    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml Cappuccino in your cup");
            
            return new Cappuccino();
        }
    }
    
    internal class ChocolateFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml Hot chocolate in your cup");
            return new Chocolate();
        }
    }
    
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            
            return new Water();
        }
    }

    public class WarmDrinkMachine
    {
        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
        new List<Tuple<string, IWarmDrinkFactory>>();

        public WarmDrinkMachine()
        {
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    namedFactories.Add(Tuple.Create(t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }
        
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }
            
            Console.WriteLine("Select a number to continue:");
            
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < namedFactories.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return namedFactories[i].Item2.Prepare(total);
                    }
                }
                
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            
            drink.Consume();
        }
    }
}
