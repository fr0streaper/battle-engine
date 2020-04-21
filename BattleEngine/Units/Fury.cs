using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Fury : Unit
    {
        public Fury() : base("Fury", 16, 5, 3, new Tuple<int, int>(5, 7), 16)
        {
            innateEffects.Add(new Effects.NoEnemyRetaliation());
        }
    }
}