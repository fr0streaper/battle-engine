using System;
using System.Collections.Generic;
using System.Text;
using BattleEngine;

namespace RogueLibrary.Effects
{
    public class Poison : Effect
    {
        int duration = 2;

        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Attack = (int)(ally.Attack * 0.9);
                ally.Defence = (int)(ally.Defence * 0.9);
            }
        }

        public override void NewRound()
        {
            --duration;
        }

        public override bool IsValid()
        {
            return duration > 0;
        }

        public override Effect Clone()
        {
            Effect result = new Effects.Poison();
            result.Owner = Owner;
            return result;
        }
    }
}
