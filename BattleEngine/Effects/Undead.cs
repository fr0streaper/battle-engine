using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class Undead : Effect
    {
        public override void Action(object sender, ActionArgs e)
        {
            if (e.ID == "UseAbility" && (e.Misc as Spell).ToReadableString() == "Revive" && e.Targets[0] == Owner)
            {
                Owner.CanBeRevived = true;
            }
        }
    }
}
