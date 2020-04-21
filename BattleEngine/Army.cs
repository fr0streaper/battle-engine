using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Army
    {
        private List<UnitsStack> stacks;

        public IList<UnitsStack> Stacks => stacks.AsReadOnly();

        public Army()
        {
            stacks = new List<UnitsStack>();
        }

        public Army(List<UnitsStack> stacks)
        {
            if (stacks == null)
            {
                throw new ArgumentNullException("Army cannot be initialized with a null list");
            }
            else
            {
                foreach (var stack in stacks)
                {
                    if (stack == null)
                    {
                        throw new ArgumentNullException("Army cannot be initialized wtih a list containing null stacks");
                    }
                }
            }

            if (stacks.Count < 1 || stacks.Count > 6)
            {
                throw new ArgumentException("Army must contain between 1 and 6 unit stacks");
            }

            this.stacks = stacks;
        }

        public bool Add(UnitsStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("Cannot add a null stack to an army");
            }

            if (stacks.Count == 6)
            {
                return false;
            }

            stacks.Add(stack);
            return true;
        }

        public void Remove(UnitsStack stack)
        {
            if (stack == null)
            {
                return;
            }

            stacks.Remove(stack);
        }
    }
}
