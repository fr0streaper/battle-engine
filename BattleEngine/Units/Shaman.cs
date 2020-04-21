using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Shaman : Unit
    {
        public Shaman() : base("Shaman", 40, 12, 10, new Tuple<int, int>(7, 12), 10.5)
        {
            spells.Add(new Spells.Haste());
        }
    }
}
