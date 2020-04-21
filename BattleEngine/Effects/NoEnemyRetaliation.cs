using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class NoEnemyRetaliation : Effect
    {
        private int storedRetaliationCount;

        public override void Action(object sender, ActionArgs e)
        {
            if (e.Actor == Owner)
            {
                if (e.ID == "Retaliation_START")
                {
                    storedRetaliationCount = e.Targets[0].RetaliationsLeft;
                    e.Targets[0].RetaliationsLeft = -1000;
                }
                else if (e.ID == "Retaliation_END")
                {
                    e.Targets[0].RetaliationsLeft = storedRetaliationCount;
                }
            }
        }

        public override Effect Clone()
        {
            return new NoEnemyRetaliation();
        }
    }
}
