using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class BattleArmy
    {
        private List<BattleUnitsStack> _stacks;

        public IList<BattleUnitsStack> Stacks => _stacks.AsReadOnly();
        public bool HasRetreated { get; private set; }

        public BattleArmy(Army army)
        {
            if (army == null)
            {
                throw new ArgumentNullException("BattleArmy cannot be initialized with a null army");
            }

            _stacks = new List<BattleUnitsStack>();
            foreach (var stack in army.Stacks)
            {
                _stacks.Add(new BattleUnitsStack(stack, this));
            }
            HasRetreated = false;
        }

        public bool Add(BattleUnitsStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("Cannot add a null BattleUnitsStack to the stack list");
            }

            if (_stacks.Count == 9)
            {
                return false;
            }

            _stacks.Add(stack);
            return true;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _stacks.Count)
            {
                throw new ArgumentOutOfRangeException("Stack index out of range");
            }

            _stacks.RemoveAt(index);
        }

        public void NewRound()
        {
            foreach (BattleUnitsStack stack in _stacks)
            {
                stack.NewRound();
            }
        }

        public void UpdateStacks()
        {
            List<BattleUnitsStack> invalid = new List<BattleUnitsStack>();

            foreach (var stack in _stacks)
            {
                if (!stack.IsAlive())
                {
                    invalid.Add(stack);
                }
            }

            foreach (var stack in invalid)
            {
                _stacks.Remove(stack);
            }
        }

        public void Retreat()
        {
            HasRetreated = true;
        }

        public bool IsAlive()
        {
            return _stacks.Count > 0;
        }
    }
}
