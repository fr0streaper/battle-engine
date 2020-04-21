using System;
using System.Collections.Generic;
using System.Text;
using BattleEngine;

namespace RogueLibrary.Spells
{
    public class PoisonStrike : Spell
    {
        public override void Cast(BattleUnitsStack caster, List<BattleUnitsStack> targets)
        {
            targets[0].AddEffect(new Effects.PoisonStrike());
        }
    }
}
