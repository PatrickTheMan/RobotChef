using Newtonsoft.Json;
using RobotChefProject._2_Domain;
using RobotChefProject._3_Models.FileHandling;
using RobotChefProject._3_Models.Graphics;
using System.Xml.Linq;

namespace RobotChefProject._3_Models
{
    public class RobotChef
    {
        #region FileHandler
        private readonly FileHandler _fileHandler;
        private readonly Visualizer _visualizer;
        #endregion
        #region Constructor
        public RobotChef(FileHandler fileHandler, Visualizer visualizer)
        {
            this._fileHandler = fileHandler;
            this._visualizer = visualizer;

            _visualizer.Display("robot_chef", "Booting ...", 10);
            _visualizer.Display("robot_chef", "Booted and ready to work!");
            Thread.Sleep(1000);
        }
        #endregion
        public void Start()
        {
            Recipe recipe = TakeOrder("Burger");

            Ingredient bun = Get("Bun", true);
            if (bun.IsFrozen) Heat(ref bun);
            Cut(ref bun);

            Ingredient meat = Get("Meat", true);
            Heat(ref meat);

            Ingredient salat = Get("Salat");
            Wash(ref salat);
            for (int i = 0; i < 5; i++)
                Cut(ref salat);

            Ingredient tomato = Get("Tomato");
            Wash(ref tomato);
            for (int i = 0; i < 3; i++)
                Cut(ref tomato);

            Ingredient cucumber = Get("Cucumber");
            Wash(ref cucumber);
            for (int i = 0; i < 3; i++)
                Cut(ref cucumber);

            Ingredient ketchup = Get("Ketchup");

            Ingredient[] ingredients = [bun, meat, salat, tomato, cucumber, ketchup, bun.Clone()];
            Food burger = Assemble("Burger", ingredients);

            int cash = Deliver(burger, recipe);

            if (cash > recipe.Price)
            {
                int change = CalculateChange(cash, recipe.Price);
                GiveChange(change);
            }
        }

        #region Actions
        #region Order and Get Ingredients
        private Recipe TakeOrder(string orderName)
        {
            _visualizer.Display("robot_chef", $"Order received: {orderName}");
            Thread.Sleep(1000); // Simulate some delay in taking the order
            return _fileHandler.GetRecipe(orderName);
        }
        private Ingredient Get(string name, bool isFrozen = false)
        {
            _visualizer.Display("robot_chef_getting", $"Getting ingredient: {name}");
            Thread.Sleep(800); // Simulate some delay in getting the ingredient
            return new() { Name = name, IsFrozen = isFrozen };
        }
        #endregion
        #region Actions on Ingredients
        private void Cut(ref Ingredient obj)
        {
            obj.IsCut = true;
            obj.CutAmount++;
            _visualizer.Display("robot_chef_cutting", $"Cutting ingredient: {obj.Name}, Cuts amount: {obj.CutAmount}", 2);
        }
        private void Heat(ref Ingredient obj)
        {
            obj.IsHeated = true;
            obj.IsFrozen = false;
            _visualizer.Display("robot_chef_heating", $"Heating ingredient: {obj.Name}", 16);
        }
        private void Wash(ref Ingredient obj)
        {
            obj.IsWashed = true;
            _visualizer.Display("robot_chef_washing", $"Washing ingredient: {obj.Name}", 6);
        }
        private void Stir(ref Ingredient obj)
        {
            obj.IsStirred = true;
            _visualizer.Display("robot_chef_stirring", $"Stirring ingredient: {obj.Name}", 6);
        }
        #endregion
        #region Assemble and Deliver
        private Food Assemble(string name, Ingredient[] objs)
        {
            _visualizer.Display("robot_chef_bowl", $"Assembling/Mixing into: {name}", 20);
            return new() { Name = name, Ingredients = [.. objs] };
        }
        private int Deliver(Food objs, Recipe recipe)
        {
            _visualizer.Display("robot_chef_delivering", $"Delivering: {objs.Name}", 10);

            objs.Ingredients[0].IsFirstAssembled = true;
            objs.Ingredients[^1].IsLastAssembled = true;

            foreach (var e in recipe.Ingredients)
            {
                bool found = false;
                bool failed = false;
                foreach (var a in objs.Ingredients)
                {
                    if (e.Name == a.Name)
                    {
                        if (e.FrozenNeeded != a.IsFrozen ||
                            e.NeedsCutting != a.IsCut ||
                            e.CutsNeeded != a.CutAmount ||
                            e.NeedsHeating != a.IsHeated ||
                            e.NeedsWashing != a.IsWashed ||
                            e.NeedsStirring != a.IsStirred ||
                            e.NeedsFirstAssembely == true && a.IsFirstAssembled != true ||
                            e.NeedsLastAssembely == true && a.IsLastAssembled != true)
                        {
                            failed = true;
                            _visualizer.Display("robot_chef", $"Delivery might have failed Actual: {JsonConvert.SerializeObject(a)}");
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
                    _visualizer.Display("robot_chef", $"Delivery failed: {objs.Name} is missing ingredient: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
                if (failed)
                {
                    _visualizer.Display("robot_chef", $"Delivery failed: {objs.Name} has wrong ingredient:\n Expected: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
            }

            _visualizer.Display("robot_chef", $"Delivery successful: {objs.Name}");
            Thread.Sleep(2000); // Simulate some delay in delivering the food
            return new Random().Next(recipe.Price, recipe.Price * 2);
        }
        #endregion
        #region Payment
        private int CalculateChange(int cash, int price)
        {
            _visualizer.Display("robot_chef", $"Calculating change: Cash received: {cash}, Price: {price}", 10);
            return cash - price;
        }
        private void GiveChange(int cash)
        {
            _visualizer.Display("robot_chef", $"Change given: {cash}", 8);
        }
        #endregion
        #endregion
        #region History
        public void History()
        {
            _visualizer.DisplayHistory();
        }
        #endregion
    }
}
