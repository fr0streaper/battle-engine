using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Arbalist : Unit
    {
        public Arbalist() : base("Arbalist", 10, 4, 4, new Tuple<int, int>(2, 8), 8)
        {
            innateEffects.Add(new Effects.Ranged());
            innateEffects.Add(new Effects.Sharpshooter());
        }
    }
}
