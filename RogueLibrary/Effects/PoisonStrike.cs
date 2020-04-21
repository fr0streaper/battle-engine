using System;
using System.Collections.Generic;
using System.Text;
using BattleEngine;

namespace RogueLibrary.Effects
{
    public class PoisonStrike : Effect
    {
        private int used = 1;

        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Attack = 0;
                ally.Defence = 0;
            }
        }

        public override void Action(object sender, ActionArgs e)
        {
            if (e.ID == "Attack_START" && e.Actor == Owner)
            {
                e.Targets[0].AddEffect(new Effects.Poison());
                --used;
            }
        }

        public override bool IsValid()
        {
            return used > 0;
        }

        public override Effect Clone()
        {
            Effect result = new Effects.PoisonStrike();
            result.Owner = Owner;
            return result;
        }
    }
}
