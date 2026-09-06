using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RobotChefProject._2_Domain;
using RobotChefProject._3_Models.FileHandling;
using Spectre.Console;

namespace RobotChefProject._3_Models
{
    public class RobotChef
    {
        private readonly FileHandler _fileHandler = new();

        public void Start()
        {
            List<Recipe> recipies = [];
            recipies.Add(TakeOrder("Burger"));
            
            Food bun = Get("Bun", isFrozen: true);
            if (bun.IsFrozen) Heat(ref bun);
            Cut(ref bun);

            Food meat = Get("Meat", isFrozen: true);
            Heat(ref meat);

            Food salat = Get("Salat");
            Wash(ref salat);
            for (int i = 0; i < 10; i++)
                Cut(ref salat);

            Food tomato = Get("Tomato");
            Wash(ref tomato);
            for (int i = 0; i < 5; i++)
                Cut(ref tomato);

            Food cucumber = Get("Cucumber");
            Wash(ref cucumber);
            for (int i = 0; i < 10; i++)
                Cut(ref cucumber);

            Food ketchup = Get("Ketchup");

            Food[] ingredients = [bun, meat, salat, tomato, cucumber, ketchup, bun.Clone()];
            Recipe burger = Assemble("Burger" ,ingredients);

            int cash = Deliver(burger, recipies[0]);

            if (cash > recipies[0].Price)
            {
                int change = CalculateChange(cash, recipies[0].Price);
                GiveChange(change);
            }
        }

        #region Actions
        private Recipe TakeOrder(string orderName)
        {
            System.Console.WriteLine($"Order received: {orderName}");
            return _fileHandler.GetRecipe(orderName);
        }

        private Food Get(string name, bool isFrozen = false)
        {
            System.Console.WriteLine($"Getting ingredient: {name}, Frozen: {isFrozen}");
            return new() { Name = name, IsFrozen = isFrozen };
        }

        private void Cut(ref Food obj)
        {
            obj.IsCut = true;
            obj.CutAmount++;
            System.Console.WriteLine($"Cutting ingredient: {obj.Name}, Cuts amount: {obj.CutAmount}");
        }

        private void Heat(ref Food obj)
        {
            System.Console.WriteLine($"Heating ingredient: {obj.Name}");
            obj.IsHeated = true;
            obj.IsFrozen = false;
        }

        private void Wash(ref Food obj)
        {
            System.Console.WriteLine($"Washing ingredient: {obj.Name}");
            obj.IsWashed = true;
        }

        private Recipe Assemble(string name, Food[] objs)
        {
            System.Console.WriteLine($"Assembling recipe: {name}");
            return new() { Name = name, Ingredients = [.. objs] };
        }

        private int Deliver(Recipe objs, Recipe recipe)
        {
            System.Console.WriteLine($"Delivering recipe: {objs.Name}");

            objs.Ingredients[0].IsFirstAssembled = true;
            objs.Ingredients[objs.Ingredients.Count-1].IsLastAssembled = true;

            foreach (var e in recipe.Ingredients)
            {
                bool found = false;
                bool failed = false;
                foreach (var a in objs.Ingredients)
                {
                    if (e.Name == a.Name)
                    {
                        if (a.IsFrozen == true ||
                            e.NeedsCutting != a.IsCut ||
                            e.CutsNeeded != a.CutAmount ||
                            e.NeedsHeating != a.IsHeated ||
                            e.NeedsWashing != a.IsWashed ||
                            e.NeedsFirstAssembely != a.IsFirstAssembled ||
                            e.NeedsLastAssembely != a.IsLastAssembled)
                        {
                            failed = true;
                        }
                        else
                        {
                            found = true;
                            failed = false;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    System.Console.WriteLine($"Delivery failed: {objs.Name} is missing ingredient: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
                if (failed)
                {
                    System.Console.WriteLine($"Delivery failed: {objs.Name} has wrong ingredient:\n Expected: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
            }

            System.Console.WriteLine($"Delivery successful: {objs.Name}");
            return new Random().Next(recipe.Price, recipe.Price * 2);
        }

        private int CalculateChange(int cash, int price)
        {
            System.Console.WriteLine($"Calculating change: Cash received: {cash}, Price: {price}");
            return cash - price;
        }

        private void GiveChange(int cash)
        {
            System.Console.WriteLine($"Change given: {cash}");
        }
        #endregion
    }
}
