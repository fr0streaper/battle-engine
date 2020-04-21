using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Griffin : Unit
    {
        public Griffin() : base("Griffin", 30, 7, 5, new Tuple<int, int>(5, 10), 15)
        {
            innateEffects.Add(new Effects.InfiniteRetaliation());
        }
    }
}
