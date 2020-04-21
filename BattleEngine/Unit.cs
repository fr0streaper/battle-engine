using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Unit
    {
        private readonly Statistics stats;
        protected readonly List<Spell> spells;
        protected readonly List<Effect> innateEffects;

        //public readonly int Type;
        public readonly string Name;
        public Statistics Stats { get { return new Statistics(stats); } }
        public IList<Spell> Spells => spells.AsReadOnly();
        public IList<Effect> InnateEffects => innateEffects.AsReadOnly();

        public Unit(string name, int hitpoints, int attack, int defence, Tuple<int, int> damage, double initiative)
        {
            //Type = type;
            Name = name;
            stats = new Statistics(hitpoints, attack, defence, damage, initiative);
            spells = new List<Spell>();
            innateEffects = new List<Effect>();
        }

    }
}
