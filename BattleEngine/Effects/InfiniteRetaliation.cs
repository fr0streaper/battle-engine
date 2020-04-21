using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class InfiniteRetaliation : Effect
    {
        public override void Action(object sender, ActionArgs e)
        {
            if (e.ID == "Retaliation_END" && e.Actor == Owner)
            {
                ++e.Actor.RetaliationsLeft;
            }
        }
    }
}
