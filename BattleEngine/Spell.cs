using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public abstract class Spell
    {
        public BattleUnitsStack Owner { get; set; }

        public abstract void Cast(BattleUnitsStack caster, List<BattleUnitsStack> targets);

        public virtual string ToReadableString()
        {
            return GetType().Name;
        }

        public virtual Spell Clone()
        {
            return this;
        }
    }
}
