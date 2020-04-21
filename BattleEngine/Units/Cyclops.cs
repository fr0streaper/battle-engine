using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Cyclops : Unit
    {
        public Cyclops() : base("Cyclops", 85, 20, 15, new Tuple<int, int>(18, 26), 10)
        {
            innateEffects.Add(new Effects.Ranged());
        }
    }
}
