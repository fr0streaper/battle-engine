using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Battle
    {
        private const double EPS = 1e-9;
        private List<BattleArmy> armies;
        private List<BattleUnitsStack> initiativeQueue, waitingQueue;

        public IList<BattleArmy> Armies => armies.AsReadOnly();
        public IList<BattleUnitsStack> InitiativeQueue => initiativeQueue.AsReadOnly();
        public IList<BattleUnitsStack> WaitingQueue => waitingQueue.AsReadOnly();
        public IList<BattleUnitsStack> TotalInitiativeQueue { get
            {
                List<BattleUnitsStack> result = new List<BattleUnitsStack>(initiativeQueue);
                result.AddRange(waitingQueue);
                return result.AsReadOnly();
            } }
        public string LastAction { get; private set; }

        //----- constructors -----

        public Battle(List<BattleArmy> armies)
        {
            if (armies == null)
            {
                throw new ArgumentNullException("Battle cannot be initialized with a null list");
            }

            if (armies.Count != 2)
            {
                throw new ArgumentException("Battle cannot be initialized with a number of armies other than 2");
            }

            foreach (BattleArmy army in armies)
            {
                if (army == null)
                {
                    throw new ArgumentNullException("Battle cannot be initialized with a list containing null stacks");
                }

                foreach (BattleUnitsStack stack in army.Stacks)
                {
                    stack.SubscribeToPewdiepie(this);
                }
            }

            this.armies = new List<BattleArmy>(armies);
            initiativeQueue = new List<BattleUnitsStack>();
            waitingQueue = new List<BattleUnitsStack>();

            foreach (var army in this.armies)
            {
                foreach (var stack in army.Stacks)
                {
                    initiativeQueue.Add(stack);
                }
            }
            UpdateStatus();
        }

        //----- event handling -----

        public event EventHandler<ActionArgs> Action;

        protected virtual void OnAction(ActionArgs e)
        {
            Action?.Invoke(this, e);
        }

        protected void PerformAction(string id, BattleUnitsStack actor, List<BattleUnitsStack> targets, object misc)
        {
            ActionArgs args = new ActionArgs(id);
            args.Actor = actor;
            args.Targets = targets;
            args.Misc = misc;
            OnAction(args);
        }

        //----- actions -----

        public void Attack(BattleUnitsStack attacker, BattleUnitsStack defender, bool isRetaliation = false)
        {
            if (attacker == null || defender == null)
            {
                throw new ArgumentNullException("Cannot attack if one of the arguments is null");
            }

            PerformAction("Attack_START", attacker, new List<BattleUnitsStack> { defender }, null);

            if (!attacker.IsAlive() || !defender.IsAlive())
            {
                attacker.ParentArmy.UpdateStacks();
                defender.ParentArmy.UpdateStacks();
                UpdateStatus();
                return;
            }

            attacker.UpdateCurrentStats();
            defender.UpdateCurrentStats();

            Statistics attackerStats = defender.DetermineEnemyStats(attacker.CurrentStats);
            Statistics defenderStats = attacker.DetermineEnemyStats(defender.CurrentStats);

            double coefficient = attacker.CurrentCount;
            if (attackerStats.Attack > defenderStats.Defence)
            {
                coefficient *= 1 + 0.05 * (attackerStats.Attack - defenderStats.Defence);
            }
            else
            {
                coefficient /= 1 + 0.05 * (defenderStats.Defence - attackerStats.Attack);
            }

            int damage = (new Random()).Next((int)(coefficient * attackerStats.Damage.Item1), (int)(coefficient * attackerStats.Damage.Item2));
            defender.ReceiveDamage(damage);

            if (!isRetaliation && defender.IsAlive() && defender.RetaliationsLeft > 0)
            {
                PerformAction("Retaliation_START", attacker, new List<BattleUnitsStack> { defender }, null);
                if (defender.IsAlive() && defender.RetaliationsLeft > 0)
                {
                    --defender.RetaliationsLeft;
                    Attack(defender, attacker, true);
                }
                PerformAction("Retaliation_END", attacker, new List<BattleUnitsStack> { defender }, null);
            }

            PerformAction("Attack_END", attacker, new List<BattleUnitsStack> { defender }, damage);

            if (!isRetaliation)
            {
                initiativeQueue.Remove(attacker);
                waitingQueue.Remove(attacker);
            }

            defender.ParentArmy.UpdateStacks();
            UpdateStatus();

            LastAction = string.Format("{0} stack attacked {1} stack; Damage: {2}", attacker.BaseStack.Type.Name, defender.BaseStack.Type.Name, damage);
        }

        public void UseAbility(BattleUnitsStack user, Spell spell, List<BattleUnitsStack> targets)
        {
            if (user == null || spell == null || targets == null || targets.FindIndex((x) => x == null) != -1)
            {
                throw new ArgumentNullException("Cannot use an ability if argument(s) is null");    
            }

            PerformAction("UseAbility", user, targets, spell);
            spell.Cast(user, targets);

            user.RemoveSpell(spell);

            initiativeQueue.Remove(user);
            waitingQueue.Remove(user);

            UpdateStatus();

            LastAction = string.Format("{0} stack cast {1} on: ", user.BaseStack.Type.Name, spell.ToReadableString());
            foreach (var target in targets)
            {
                LastAction += target.BaseStack.Type.Name + "; ";
            }
        }

        public void Wait(BattleUnitsStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("Cannot wait if the target stack is null");
            }

            if (initiativeQueue.Contains(stack))
            {
                PerformAction("Wait", stack, new List<BattleUnitsStack>(), null);
                initiativeQueue.Remove(stack);
                waitingQueue.Add(stack);
                UpdateStatus();
            }
            else if (!waitingQueue.Contains(stack))
            {
                throw new ArgumentException("Cannot wait if the target is not in the queue");
            }

            LastAction = string.Format("{0} stack waits", stack.BaseStack.Type.Name);
        }

        public void Defend(BattleUnitsStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("Null stack cannot defend");
            }

            PerformAction("Defend", stack, new List<BattleUnitsStack>(), null);
            stack.AddEffect(new Effects.Defending());

            initiativeQueue.Remove(stack);
            waitingQueue.Remove(stack);

            UpdateStatus();

            LastAction = string.Format("{0} stack defended", stack.BaseStack.Type.Name);
        }

        public void Retreat(BattleUnitsStack stack)
        {
            if (stack == null)
            {
                throw new ArgumentNullException("Cannot retreat using a null BattleUnitsStack");
            }

            PerformAction("Retreat", stack, new List<BattleUnitsStack>(), null);
            stack.ParentArmy.Retreat();
            UpdateStatus();
        }

        //----- initiative management -----

        public BattleUnitsStack NextActor()
        {
            if (initiativeQueue.Count > 0)
            {
                return initiativeQueue[0];
            }
            else
            {
                return waitingQueue[0];
            }
        }

        public void UpdateStatus()
        {
            List<BattleUnitsStack> outOfAction = new List<BattleUnitsStack>();
            foreach (var army in armies)
            {
                foreach (var stack in army.Stacks)
                {
                    stack.UpdateCurrentStats();
                    if (!stack.IsAlive())
                    {
                        outOfAction.Add(stack);
                    }
                }
            }

            foreach (var stack in outOfAction)
            {
                initiativeQueue.Remove(stack);
                waitingQueue.Remove(stack);
            }

            initiativeQueue.Sort((x, y) => y.CurrentStats.Initiative.CompareTo(x.CurrentStats.Initiative));
            waitingQueue.Sort((x, y) => x.CurrentStats.Initiative.CompareTo(y.CurrentStats.Initiative));
        }

        public void NewRound()
        {
            List<BattleUnitsStack> result = new List<BattleUnitsStack>();

            foreach (var army in armies)
            {
                foreach (var stack in army.Stacks)
                {
                    if (stack.IsAlive())
                    {
                        result.Add(stack);
                    }
                }
            }

            foreach (var stack in result)
            {
                stack.NewRound();
                stack.UpdateCurrentStats();
            }

            waitingQueue.Clear();
            initiativeQueue = result;

            UpdateStatus();
        }

        public IList<BattleUnitsStack> NextRoundInitiativeQueue()
        {
            List<BattleUnitsStack> result = new List<BattleUnitsStack>();

            foreach (var army in armies)
            {
                foreach (var stack in army.Stacks)
                {
                    if (stack.IsAlive())
                    {
                        result.Add(new BattleUnitsStack(stack));
                    }
                }
            }

            foreach (var stack in result)
            {
                stack.NewRound();
                stack.UpdateCurrentStats();
            }

            Random rand = new Random();
            result.OrderBy(x => rand.Next());
            result.Sort((x, y) => y.CurrentStats.Initiative.CompareTo(x.CurrentStats.Initiative));

            return result.AsReadOnly();
        }

        //----- battle status reporting -----

        public int IsOver()
        {
            int lastNotDead = -1;
            int count = 0;

            for (int i = 0; i < armies.Count; ++i)
            {
                if (armies[i].IsAlive() && !armies[i].HasRetreated)
                {
                    ++count;
                    lastNotDead = i;
                }
            }

            if (count > 1)
            {
                return -1; //not over
            }
            if (count == 0)
            {
                return 0; //all dead
            }

            return lastNotDead + 1; //winner
        }
    }
}