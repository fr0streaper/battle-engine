using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class BoneDragon : Unit
    {
        public BoneDragon() : base("Bone Dragon", 150, 27, 28, new Tuple<int, int>(15, 30), 11)
        {
            innateEffects.Add(new Effects.Undead());
            spells.Add(new Spells.Curse());
        }
    }
}
