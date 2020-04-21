using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Units
{
    public class Angel : Unit
    {
        public Angel() : base("Angel", 180, 27, 27, new Tuple<int, int>(45, 45), 11)
        {
            spells.Add(new Spells.PunishmentStrike());
        }
    }
}
