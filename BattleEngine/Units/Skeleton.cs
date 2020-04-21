using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Skeleton : Unit
    {
        public Skeleton() : base("Skeleton", 5, 1, 2, new Tuple<int, int>(1, 1), 10)
        {
            innateEffects.Add(new Effects.Undead());
        }
    }
}
