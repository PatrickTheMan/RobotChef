using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotChefProject._2_Domain
{
    public class Mix
    {
        public required string Name { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
    }
}
