using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotChefProject._2_Domain
{
    public class RecipeIngredient
    {
        public required string Name { get; set; }

        public bool FrozenNeeded { get; set; } = false;
        public bool NeedsCutting { get; set; } = false;
        public int CutsNeeded { get; set; } = 0;
        public bool NeedsHeating { get; set; } = false;
        public bool NeedsWashing { get; set; } = false;
        public bool NeedsStirring { get; set; } = false;
        public bool NeedsFirstAssembely { get; set; } = false;
        public bool NeedsLastAssembely { get; set; } = false;

        public RecipeIngredient Clone()
        {
            return (RecipeIngredient)this.MemberwiseClone();
        }
    }
}
