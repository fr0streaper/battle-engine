using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace BattleEngine
{
    public class Program
    {
        private static Battle battle;

        private static string fail()
        {
            Console.Beep();
            UI.RenderBattle(battle);
            return Console.ReadLine();
        }

        private static Army commenceRecruitment(List<Type> availableUnitTypes)
        {
            List<Unit> availableUnits = new List<Unit>();
            foreach (var type in availableUnitTypes)
            {
                availableUnits.Add((Unit)Activator.CreateInstance(type));
            }

            List<UnitsStack> currentlyRecruited = new List<UnitsStack>();
            UI.RenderRecruitment(availableUnits, currentlyRecruited);
            string cmd = Console.ReadLine();
            while (cmd != "Finish" || currentlyRecruited.Count == 0)
            {
                string[] items = cmd.Split();

                if (items.Length < 2 || (items[0] != "Add" && items[0] != "Remove"))
                {
                    Console.Beep();
                    UI.RenderRecruitment(availableUnits, currentlyRecruited);
                    cmd = Console.ReadLine();
                    continue;
                }

                int id;
                if (!int.TryParse(items[1], out id))
                {
                    Console.Beep();
                    UI.RenderRecruitment(availableUnits, currentlyRecruited);
                    cmd = Console.ReadLine();
                    continue;
                }

                if (items[0] == "Add")
                {
                    try
                    {
                        int count = int.Parse(items[2]);
                        UnitsStack stack = new UnitsStack(availableUnits[id - 1], count);
                        if (currentlyRecruited.Count < 6)
                        {
                            currentlyRecruited.Add(stack);
                        }
                    }
                    catch (Exception)
                    {
                        Console.Beep();
                        UI.RenderRecruitment(availableUnits, currentlyRecruited);
                        cmd = Console.ReadLine();
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        currentlyRecruited.RemoveAt(id - 1);
                    }
                    catch (Exception)
                    {
                        Console.Beep();
                        UI.RenderRecruitment(availableUnits, currentlyRecruited);
                        cmd = Console.ReadLine();
                        continue;
                    }
                }

                UI.RenderRecruitment(availableUnits, currentlyRecruited);
                cmd = Console.ReadLine();
            }

            return new Army(currentlyRecruited);
        }

        private static void commenceBattle(Army army1, Army army2)
        {
            List<BattleArmy> armies = new List<BattleArmy> { new BattleArmy(army1), new BattleArmy(army2) };
            battle = new Battle(armies);

            UI.RenderBattle(battle);
            string cmd = Console.ReadLine();
            while (cmd != "Quit")
            {
                string[] items = cmd.Split();

                if (items.Length < 1)
                {
                    cmd = fail();
                    continue;
                }

                string action = items[0];
                    
                if (battle.TotalInitiativeQueue.Count == 0)
                {
                    battle.NewRound();
                }

                BattleUnitsStack actor = battle.NextActor();
                BattleUnitsStack target = null;
                int action_id = 0;

                if (action == "Attack" || action == "UseAbility")
                {
                    try
                    {
                        int input_start_id = 1;
                        if (action == "UseAbility")
                        {
                            action_id = int.Parse(items[input_start_id]);
                            ++input_start_id;
                        }

                        int ta_id = int.Parse(items[input_start_id]) - 1;
                        int t_id = int.Parse(items[input_start_id + 1]) - 1;

                        target = battle.Armies[ta_id].Stacks[t_id];
                    }
                    catch (Exception)
                    {
                        cmd = fail();
                        continue;
                    }
                }

                if (action == "Attack")
                {
                    battle.Attack(actor, target);
                }
                else if (action == "UseAbility")
                {
                    try
                    {
                        battle.UseAbility(actor, actor.BaseStack.Type.Spells[action_id - 1], new List<BattleUnitsStack> { target });
                    }
                    catch (Exception)
                    {
                        cmd = fail();
                        continue;
                    }
                }
                else if (action == "Defend")
                {
                    battle.Defend(actor);
                }
                else if (action == "Wait")
                {
                    battle.Wait(actor);
                }
                else if (action == "Retreat")
                {
                    battle.Retreat(actor);
                }
                else
                {
                    cmd = fail();
                    continue;
                }

                int status = battle.IsOver();
                if (status >= 0)
                {
                    UI.RenderBattleOverScreen(status);
                    break;
                }

                if (battle.TotalInitiativeQueue.Count == 0)
                {
                    battle.NewRound();
                }

                UI.RenderBattle(battle);
                cmd = Console.ReadLine();
            }
        }

        private static void printInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            List<Assembly> assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Mods\\", "*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(file));
            }

            List<Type> availableUnitTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                foreach (var x in assembly.GetTypes().ToArray().Where(type => type.IsSubclassOf(typeof(Unit))))
                {
                    availableUnitTypes.Add(x);
                }
            }

            Army army1 = commenceRecruitment(availableUnitTypes);
            Army army2 = commenceRecruitment(availableUnitTypes);
            commenceBattle(army1, army2);
        }
    }
}
