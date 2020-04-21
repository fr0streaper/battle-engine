using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class BattleUnitsStack
    {
        private List<Effect> effects;
        private List<Spell> spells;

        public readonly UnitsStack BaseStack;
        public Battle battle { get; private set; }
        public int CurrentCount { get; private set; }
        public int LastHP { get; private set; }
        public Statistics CurrentStats { get; private set; }
        public BattleArmy ParentArmy { get; private set; }
        public int RetaliationsLeft { get; set; }
        public IList<Effect> Effects => effects.AsReadOnly();
        public IList<Spell> Spells => spells.AsReadOnly();
        public bool CanBeRevived { get; set; }

        //----- constructors -----

        public BattleUnitsStack(UnitsStack stack, BattleArmy parentArmy)
        {
            BaseStack = stack;
            CurrentCount = stack.Count;
            LastHP = stack.Type.Stats.HitPoints;
            CurrentStats = stack.Type.Stats;
            ParentArmy = parentArmy;
            RetaliationsLeft = 1;

            effects = new List<Effect>();
            foreach (var effect in stack.Type.InnateEffects)
            {
                effects.Add(effect.Clone());
                effects.Last().Owner = this;
            }

            spells = new List<Spell>();
            foreach (var spell in stack.Type.Spells)
            {
                spells.Add(spell.Clone());
                spells.Last().Owner = this;
            }
        }

        public BattleUnitsStack(BattleUnitsStack stack)
        {
            BaseStack = stack.BaseStack;
            CurrentCount = stack.CurrentCount;
            LastHP = stack.LastHP;
            CurrentStats = new Statistics(stack.CurrentStats);            
            ParentArmy = stack.ParentArmy;

            effects = new List<Effect>();
            foreach (var effect in stack.Effects)
            {
                effects.Add(effect.Clone());
                effects.Last().Owner = this;
            }

            spells = new List<Spell>();
            foreach (var spell in stack.Spells)
            {
                spells.Add(spell.Clone());
                spells.Last().Owner = this;
            }
        }

        //----- event handling -----
        
        public void SubscribeToPewdiepie(Battle battle)
        {
            this.battle = battle;

            foreach (Effect effect in effects)
            {
                this.battle.Action += effect.Action;
            }
        }

        //----- effect management -----

        private void UpdateEffects()
        {
            List<Effect> invalid = new List<Effect>();

            foreach (Effect e in effects) 
            {
                if (!e.IsValid())
                {
                    invalid.Add(e);
                }
            }

            foreach (Effect e in invalid)
            {
                RemoveEffect(e);
            }
        }

        public void AddEffect(Effect effect)
        {
            effect.Owner = this;
            effects.Add(effect);
            battle.Action += effect.Action;
        }

        public void RemoveEffect(Effect effect)
        {
            effects.Remove(effect);
            try
            {
                battle.Action -= effect.Action;
            }
            catch (Exception) { }
        }

        //----- spell management -----

        public void AddSpell(Spell spell)
        {
            spell.Owner = this;
            spells.Add(spell);
        }

        public void RemoveSpell(Spell spell)
        {
            spells.Remove(spell);
        }

        //----- stat management -----

        public void ReceiveDamage(int damage)
        {
            if (damage < LastHP)
            {
                LastHP -= damage;
                return;
            }

            int unitHP = CurrentStats.HitPoints;

            damage -= LastHP;
            CurrentCount -= 1 + (damage / unitHP);

            if (CurrentCount <= 0)
            {
                CurrentCount = 0;
                LastHP = 0;
                return;
            }

            LastHP = unitHP - (damage % unitHP);
        }

        public void Revive(int hp)
        {
            int unitHP = CurrentStats.HitPoints;

            CurrentCount = Math.Min(CurrentCount + hp / unitHP, BaseStack.Count);
            LastHP = Math.Min(LastHP + (hp % unitHP), unitHP);
        }

        public void UpdateCurrentStats()
        {
            Statistics result = BaseStack.Type.Stats;

            UpdateEffects();
            foreach (Effect e in effects)
            {
                e.Modify(result, null);
            }

            CurrentStats = result;
        }

        public Statistics DetermineEnemyStats(Statistics enemyStats)
        {
            Statistics result = new Statistics(enemyStats);

            UpdateEffects();
            foreach (Effect e in effects)
            {
                e.Modify(null, result);
            }

            return result;
        }

        //----- miscellaneous -----

        public void NewRound()
        {
            foreach (Effect e in effects)
            {
                e.NewRound();
            }

            RetaliationsLeft = 1;
        }

        public bool IsAlive()
        {
            return CurrentCount > 0;
        }
    }
}
