using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Haste : Effect
    {
        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Initiative *= 1.4;
            }
        }
    }
}
