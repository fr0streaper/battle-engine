using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Hydra : Unit
    {
        public Hydra() : base("Hydra", 80, 15, 12, new Tuple<int, int>(7, 14), 7)
        {
            innateEffects.Add(new Effects.SplashAttack());
            innateEffects.Add(new Effects.NoEnemyRetaliation());
        }
    }
}
