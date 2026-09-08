using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotChefProject._2_Domain
{
    public enum IngredientNames
    {
        Salat,
        Tomato,
        Cucumber,
        Onion,
        Pepper,
        Carrot,
        Potato,
        Chicken,
        Meat,
        Fish,
        Rice,
        Pasta,
        Cheese,
        Egg,
        Milk,
        Butter,
        Flour,
        Sugar,
        Salt,
        Bun,
        Water,
        Ketchup
    }
    public class Ingredient
    {

        public required string Name { get; set; }

        public bool IsFrozen { get; set; } = false;
        public bool IsCut { get; set; } = false;
        public int CutAmount { get; set; } = 0;
        public bool IsHeated { get; set; } = false;
        public bool IsWashed { get; set; } = false;
        public bool IsStirred { get; set; } = false;
        public bool IsFirstAssembled { get; set; } = false;
        public bool IsLastAssembled { get; set; } = false;

        public Ingredient Clone()
        {
            return (Ingredient)this.MemberwiseClone();
        }

    }
}
