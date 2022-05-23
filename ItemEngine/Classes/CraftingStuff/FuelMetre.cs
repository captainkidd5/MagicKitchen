using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemEngine.Classes.CraftingStuff
{
    public class FuelMetre
    {
        public int CurrentFuel { get; set; }
        public int MaxFuel { get; set; }

        public bool Empty => CurrentFuel <= 0;

        public void AddFuel(int amt)
        {
            CurrentFuel += amt;
        }
    }
}
