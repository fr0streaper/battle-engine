using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine.Effects
{
    public class SplashAttack : Effect
    {
        public override void Action(object sender, ActionArgs e)
        {
            if (e.ID == "Attack_END" && e.Actor == Owner)
            {
                int damage = (int)e.Misc;
                
                foreach (var stack in e.Targets[0].ParentArmy.Stacks)
                {
                    if (stack == e.Targets[0])
                    {
                        continue;
                    } 

                    stack.ReceiveDamage(damage / 2);
                }
            }
        }
    }
}
