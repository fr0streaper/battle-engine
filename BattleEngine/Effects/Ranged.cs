using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Ranged : Effect
    {
        private int storedRetaliationCount;

        public override void Action(object sender, ActionArgs e)
        {
            if ((e.ID == "Retaliation_START" || e.ID == "Retaliation_END") && 
                (e.Actor == Owner || e.Targets[0] == Owner))
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
            return new Ranged();
        }
    }
}
