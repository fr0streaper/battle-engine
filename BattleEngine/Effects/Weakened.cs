using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Weakened : Effect
    {
        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Defence = Math.Max(ally.Defence - 12, 0);
            }
        }
    }
}
