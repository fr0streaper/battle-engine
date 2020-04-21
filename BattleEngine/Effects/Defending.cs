using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Defending : Effect
    {
        private int duration = 1;

        public override bool IsValid()
        {
            return duration > 0;
        }

        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Defence = (int)Math.Ceiling(1.3 * ally.Defence);
            }
        }

        public override void NewRound()
        {
            --duration;
        }

        public override Effect Clone()
        {
            Effect result = new Effects.Defending();
            result.Owner = Owner;
            return result;
        }
    }
}
