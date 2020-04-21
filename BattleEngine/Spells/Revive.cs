using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Spells
{
    public class Revive : Spell
    {
        public override void Cast(BattleUnitsStack caster, List<BattleUnitsStack> targets)
        {
            if (targets[0].CanBeRevived)
            {
                targets[0].Revive(100 * caster.CurrentCount);
                targets[0].CanBeRevived = false;
            }
        }
    }
}