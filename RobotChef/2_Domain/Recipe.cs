using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotChefProject._2_Domain
{
    public enum RecipeNames
    {
        Salad,
        Burger,
        Bmo,
        TomatoSauce
    }
    public class Recipe
    {
        public required string Name { get; set; }
        public int Price { get; set; }
        public required List<RecipeIngredient> Ingredients { get; set; }
    }
}
