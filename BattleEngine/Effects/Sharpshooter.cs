using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Sharpshooter : Effect
    {
        public override void Modify(Statistics ally, Statistics enemy)
        {
            if (enemy != null)
            {
                enemy.Defence = 0;
            }
        }
    }
}
