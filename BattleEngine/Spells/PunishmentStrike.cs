using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Spells
{
    public class PunishmentStrike : Spell
    {
        public override void Cast(BattleUnitsStack caster, List<BattleUnitsStack> targets)
        {
            targets[0].AddEffect(new Effects.PunishmentStrike());
        }
    }
}
