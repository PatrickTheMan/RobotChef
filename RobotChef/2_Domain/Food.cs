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

        public bool IsFrozen { get; set; } = false;

        public bool IsCut { get; set; } = false;
        public int CutAmount { get; set; } = 0;
        public bool NeedsCutting { get; set; } = false;
        public int CutsNeeded { get; set; } = 0;

        public bool IsHeated { get; set; } = false;
        public bool NeedsHeating { get; set; } = false;

        public bool IsWashed { get; set; } = false;
        public bool NeedsWashing { get; set; } = false;

        public bool IsFirstAssembled { get; set; } = false;
        public bool NeedsFirstAssembely { get; set; } = false;

        public bool IsLastAssembled { get; set; } = false;
        public bool NeedsLastAssembely { get; set; } = false;

        public Food Clone()
        {
            return (Food)this.MemberwiseClone();
        }

    }
}
