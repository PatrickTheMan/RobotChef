using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotChefProject._2_Domain
{
    public class Food
    {
        public required string Name { get; set; }
        public int Price { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
    }
}
