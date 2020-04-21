using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class UnitsStack
    {
        public readonly Unit Type;
        public readonly int Count;

        public UnitsStack(Unit type, int count)
        {
            if (count < 1 || count >= 1000000)
            {
                throw new ArgumentOutOfRangeException("A number of units in a stack must be between 1 and 999999");
            }
            if (type == null)
            {
                throw new ArgumentNullException("Cannot create a unit stack with null unit type");
            }

            Type = type;
            Count = count;
        }
    }
}
