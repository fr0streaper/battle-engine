using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public static class UI
    {
        private static void printInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        private static void printUnitStats(Unit unit, ConsoleColor armyColor = ConsoleColor.White, Statistics customStats = null, IList<Effect> customEffects = null)
        {
            Statistics stats = unit.Stats;
            if (customStats != null)
            {
                stats = customStats;
            }
            IList<Effect> effects = unit.InnateEffects;
            if (customEffects != null)
            {
                effects = customEffects;
            }

            printInColor(string.Format(" {0} ", unit.Name), armyColor);
            printInColor(string.Format("~ HP {0} ATK {1} DEF {2} DMG {3}-{4} INIT {5} ~", stats.HitPoints, stats.Attack, stats.Defence,
                stats.Damage.Item1, stats.Damage.Item2, stats.Initiative.ToString("0.0")), ConsoleColor.Gray);

            Console.WriteLine("");
            printInColor("^", ConsoleColor.Gray);
            printInColor("Effects: ", ConsoleColor.Gray);
            foreach (var effect in effects)
            {
                printInColor(effect.ToReadableString() + " ", ConsoleColor.DarkMagenta);
            }

            Console.WriteLine("");
            printInColor("^", ConsoleColor.Gray);
            printInColor("Spells: ", ConsoleColor.Gray);
            foreach (var spell in unit.Spells)
            {
                printInColor(spell.ToReadableString() + " ", ConsoleColor.DarkMagenta);
            }
        }

        private static void printStackStatus(int id, BattleUnitsStack stack, ConsoleColor armyColor)
        {
            Statistics stats = stack.CurrentStats;

            printInColor(string.Format("{0}) ", id), ConsoleColor.Gray);
            printInColor(string.Format("[{0}/{1} units] [{2} HP]", stack.CurrentCount, stack.BaseStack.Count, stack.LastHP), ConsoleColor.DarkRed);
            /*printInColor(string.Format(" {0} ", stack.BaseStack.Type.Name), armyColor);
            printInColor(string.Format("~ HP {0} ATK {1} DEF {2} DMG {3}-{4} INIT {5} ~", stats.HitPoints, stats.Attack, stats.Defence,
                stats.Damage.Item1, stats.Damage.Item2, stats.Initiative.ToString("0.0")), ConsoleColor.Gray);

            Console.WriteLine("");
            printInColor("^", ConsoleColor.Gray);
            printInColor("Effects: ", ConsoleColor.Gray);
            foreach (var effect in stack.Effects)
            {
                printInColor(effect.ToReadableString() + " ", ConsoleColor.DarkMagenta);
            }

            Console.WriteLine("");
            printInColor("^", ConsoleColor.Gray);
            printInColor("Spells: ", ConsoleColor.Gray);
            foreach (var spell in stack.Spells)
            {
                printInColor(spell.ToReadableString() + " ", ConsoleColor.DarkMagenta);
            }*/
            printUnitStats(stack.BaseStack.Type, armyColor, stats, stack.Effects);

            Console.WriteLine("");
        }

        private static void printQueue(Battle battle, IList<BattleUnitsStack> queue, ConsoleColor armyColor1, ConsoleColor armyColor2)
        {
            for (int i = 0; i < queue.Count; ++i)
            {
                BattleUnitsStack stack = queue[i];
                ConsoleColor color;

                if (stack.ParentArmy == battle.Armies[0])
                {
                    color = armyColor1;
                }
                else
                {
                    color = armyColor2;
                }

                printInColor(stack.BaseStack.Type.Name, color);
                if (i < queue.Count - 1)
                {
                    Console.Write(" -> ");
                }
            }
        }

        private static void printDelimiter(int repeats)
        {
            string result = new string('-', 3 * repeats);
            printInColor(result, ConsoleColor.DarkGray);
            Console.WriteLine("");
        }

        private static void printArmy(BattleArmy army, int id, ConsoleColor color)
        {
            printInColor(string.Format("Army {0}", id), ConsoleColor.White);
            Console.WriteLine("");
            for (int i = 0; i < army.Stacks.Count; ++i)
            {
                printStackStatus(i + 1, army.Stacks[i], color);
            }
            printDelimiter(1);
        }

        public static void RenderBattle(Battle battle)
        {
            Console.Clear();

            List<ConsoleColor> armyColors = new List<ConsoleColor> { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow };
            
            //----- initiative -----

            printInColor("Initiative queue: this round | next round", ConsoleColor.White);
            Console.WriteLine("");
            printQueue(battle, battle.TotalInitiativeQueue, armyColors[0], armyColors[1]);
            printInColor(" | ", ConsoleColor.White);
            printQueue(battle, battle.NextRoundInitiativeQueue(), armyColors[0], armyColors[1]);
            Console.WriteLine("");
            printInColor(battle.LastAction, ConsoleColor.DarkGray);
            Console.WriteLine("");

            //----- armies -----
            printDelimiter(10);

            printArmy(battle.Armies[0], 1, armyColors[0]);
            printArmy(battle.Armies[1], 2, armyColors[1]);

            printInColor("Please, enter a command in a following format: <action name> <actor army id> <actor id> <target army id> <target id> OR Quit", ConsoleColor.White);
            Console.WriteLine("");
        }

        public static void RenderRecruitment(List<Unit> availableUnits, List<UnitsStack> currentlyRecruited)
        {
            Console.Clear();

            printInColor("Available units:", ConsoleColor.Cyan);
            Console.WriteLine("");

            for (int i = 0; i < availableUnits.Count; ++i)
            {
                printInColor(string.Format("{0}) ", i + 1), ConsoleColor.White);
                printUnitStats(availableUnits[i]);
                Console.WriteLine("");
            }

            printDelimiter(3);

            printInColor("Currently recruited:", ConsoleColor.Cyan);
            Console.WriteLine("");

            for (int i = 0; i < currentlyRecruited.Count; ++i)
            {
                printInColor(string.Format("{0}) ", i + 1), ConsoleColor.White);
                printInColor(string.Format("[{0} units] ", currentlyRecruited[i].Count), ConsoleColor.DarkMagenta);
                printUnitStats(currentlyRecruited[i].Type);
                Console.WriteLine("");
            }

            printInColor("Please, enter a command in a following format: <action name> <id> [<count>] OR Finish", ConsoleColor.White);
            Console.WriteLine("");
        }

        public static void RenderBattleOverScreen(int status)
        {
            if (status == 0)
            {
                printInColor("The battle is over; everyone died", ConsoleColor.Red);
            }
            else
            {
                List<ConsoleColor> armyColors = new List<ConsoleColor> { ConsoleColor.DarkCyan, ConsoleColor.DarkYellow };
                printInColor(string.Format("The battle is over; army {0} has won", status), armyColors[status - 1]);
                Console.WriteLine("");
            }
        }
    }
}
