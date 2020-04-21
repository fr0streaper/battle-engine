using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Devil : Unit
    {
        public Devil() : base("Devil", 166, 27, 25, new Tuple<int, int>(36, 66), 11)
        {
            spells.Add(new Spells.Weaken());
        }
    }
}
