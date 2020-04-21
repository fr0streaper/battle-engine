using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class PunishmentStrike : Effect
    {
        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (ally != null)
            {
                ally.Attack += 12;
            }
        }
    }
}
