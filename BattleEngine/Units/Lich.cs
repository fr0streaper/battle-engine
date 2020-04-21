using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Lich : Unit
    {
        public Lich() : base("Lich", 50, 15, 15, new Tuple<int, int>(12, 17), 10)
        {
            innateEffects.Add(new Effects.Undead());
            innateEffects.Add(new Effects.Ranged());
            spells.Add(new Spells.Revive());
        }
    }
}
