using Newtonsoft.Json;
using RobotChefProject._2_Domain;
using RobotChefProject._3_Models.FileHandling;
using RobotChefProject._3_Models.Graphics;
using System.Runtime.InteropServices;

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
            Recipe recipe = TakeOrder(RecipeNames.Burger);

            Ingredient bunBottom = Get(IngredientNames.Bun, true);
            if (bunBottom.IsFrozen) Heat(ref bunBottom);
            Ingredient bunTop = Get(IngredientNames.Bun, true);
            if (bunTop.IsFrozen) Heat(ref bunTop);

            Ingredient meat = Get(IngredientNames.Meat, true);
            Heat(ref meat);

            Ingredient salat = Get(IngredientNames.Salat);
            Wash(ref salat);
            for (int i = 0; i < 5; i++)
                Cut(ref salat);

            Ingredient tomato = Get(IngredientNames.Tomato);
            Wash(ref tomato);
            for (int i = 0; i < 3; i++)
                Cut(ref tomato);

            Ingredient cucumber = Get(IngredientNames.Cucumber);
            Wash(ref cucumber);
            for (int i = 0; i < 3; i++)
                Cut(ref cucumber);

            Ingredient ketchup = Get(IngredientNames.Ketchup);

            Ingredient[] ingredients = [bunBottom, meat, salat, tomato, cucumber, ketchup, bunTop];
            Food burger = Prepare(RecipeNames.Burger, ingredients);

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
        private Recipe TakeOrder(RecipeNames orderName)
        {
            return TakeOrder(orderName.ToString());
        }
        private Ingredient Get(string name, bool isFrozen = false)
        {
            _visualizer.Display("robot_chef_getting", $"Getting ingredient: {name}");
            Thread.Sleep(800); // Simulate some delay in getting the ingredient
            return new() { Name = name, IsFrozen = isFrozen };
        }
        private Ingredient Get(IngredientNames name, bool isFrozen = false)
        {
            return Get(name.ToString(), isFrozen);
        }
        #endregion
        #region Actions on Ingredients
        private void Cut(ref Ingredient ingredient)
        {
            ingredient.IsCut = true;
            ingredient.CutAmount++;
            _visualizer.Display("robot_chef_cutting", $"Cutting ingredient: {ingredient.Name}, Cuts amount: {ingredient.CutAmount}", 2);
        }
        private void Heat(ref Ingredient ingredient)
        {
            ingredient.IsHeated = true;
            ingredient.IsFrozen = false;
            _visualizer.Display("robot_chef_heating", $"Heating ingredient: {ingredient.Name}", 16);
        }
        private void Wash(ref Ingredient ingredient)
        {
            ingredient.IsWashed = true;
            _visualizer.Display("robot_chef_washing", $"Washing ingredient: {ingredient.Name}", 6);
        }
        private void Stir(ref Ingredient ingredient)
        {
            ingredient.IsStirred = true;
            _visualizer.Display("robot_chef_stirring", $"Stirring ingredient: {ingredient.Name}", 6);
        }
        private void Freeze(ref Ingredient ingredient)
        {
            ingredient.IsFrozen = true;
            ingredient.IsHeated = false;
            _visualizer.Display("robot_chef_getting", $"Freezing ingredient: {ingredient.Name}", 20);
        }
        private Mix Mix(string name, Ingredient[] ingredients)
        {
            _visualizer.Display("robot_chef_bowl", $"Mixing ingredients into: {name}", 8);
            return new Mix() { Name = name, Ingredients = [.. ingredients] };
        }
        #endregion
        #region Actions on Mix
        public void Cut(ref Mix mix)
        {
            Span<Ingredient> listSpan = CollectionsMarshal.AsSpan(mix.Ingredients);
            for (int i = 0; i < mix.Ingredients.Count; i++)
            {
                listSpan[i].IsCut = true;
                listSpan[i].CutAmount++;
            }
            _visualizer.Display("robot_chef_cutting", $"Cutting mix: {mix.Name}, Cuts amount: ({string.Join(", ", mix.Ingredients.Select(i => i.CutAmount).ToArray())})", 2);
        }
        private void Heat(ref Mix mix)
        {
            Span<Ingredient> listSpan = CollectionsMarshal.AsSpan(mix.Ingredients);
            for (int i = 0; i < mix.Ingredients.Count; i++)
            {
                listSpan[i].IsHeated = true;
                listSpan[i].IsFrozen = false;
            }
            _visualizer.Display("robot_chef_heating", $"Heating mix: {mix.Name}", 16);
        }
        private void Wash(ref Mix mix)
        {
            Span<Ingredient> listSpan = CollectionsMarshal.AsSpan(mix.Ingredients);
            for (int i = 0; i < mix.Ingredients.Count; i++)
            {
                listSpan[i].IsWashed = true;
            }
            _visualizer.Display("robot_chef_washing", $"Washing mix: {mix.Name}", 6);
        }
        private void Stir(ref Mix mix)
        {
            Span<Ingredient> listSpan = CollectionsMarshal.AsSpan(mix.Ingredients);
            for (int i = 0; i < mix.Ingredients.Count; i++)
            {
                listSpan[i].IsStirred = true;
            }
            _visualizer.Display("robot_chef_stirring", $"Stirring mix: {mix.Name}", 6);
        }
        private void Freeze(ref Mix mix)
        {
            Span<Ingredient> listSpan = CollectionsMarshal.AsSpan(mix.Ingredients);
            for (int i = 0; i < mix.Ingredients.Count; i++)
            {
                listSpan[i].IsFrozen = true;
                listSpan[i].IsHeated = false;
            }
            _visualizer.Display("robot_chef_getting", $"Freezing mix: {mix.Name}", 20);
        }
        #endregion
        #region Assemble and Deliver
        private Food Prepare(string name, Ingredient[] ingredients)
        {
            _visualizer.Display("robot_chef_bowl", $"Preparing: {name}", 20);
            return new() { Name = name, Ingredients = [.. ingredients] };
        }
        private Food Prepare(RecipeNames names, Ingredient[] ingredients)
        {
            return Prepare(names.ToString(), ingredients);
        }
        private Food Prepare(string name, Mix mix, Ingredient[] ingredients)
        {
            return Prepare(name, [..mix.Ingredients, ..ingredients]);
        }
        private Food Prepare(RecipeNames names, Mix mix, Ingredient[] ingredients)
        {
            return Prepare(names.ToString(), mix, ingredients);
        }
        private int Deliver(Food food, Recipe recipe)
        {
            _visualizer.Display("robot_chef_delivering", $"Delivering: {food.Name}", 10);

            food.Ingredients[0].IsFirstAssembled = true;
            food.Ingredients[^1].IsLastAssembled = true;

            foreach (var e in recipe.Ingredients)
            {
                bool found = false;
                bool failed = false;
                foreach (var a in food.Ingredients)
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
                    _visualizer.Display("robot_chef", $"Delivery failed: {food.Name} is missing ingredient: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
                if (failed)
                {
                    _visualizer.Display("robot_chef", $"Delivery failed: {food.Name} has wrong ingredient:\n Expected: {JsonConvert.SerializeObject(e)}");
                    return 0;
                }
            }

            _visualizer.Display("robot_chef", $"Delivery successful: {food.Name}");
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
