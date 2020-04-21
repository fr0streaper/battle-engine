using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Statistics
    {
        public int HitPoints { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public Tuple<int, int> Damage { get; set; }
        public double Initiative { get; set; }

        public Statistics(int hitpoints, int attack, int defence, Tuple<int, int> damage, double initiative)
        {
            HitPoints = hitpoints;
            Attack = attack;
            Defence = defence;
            Damage = damage;
            Initiative = initiative;
        }

        public Statistics(Statistics other)
        {
            HitPoints = other.HitPoints;
            Attack = other.Attack;
            Defence = other.Defence;
            Damage = new Tuple<int, int>(other.Damage.Item1, other.Damage.Item2);
            Initiative = other.Initiative;
        }
    }
}
