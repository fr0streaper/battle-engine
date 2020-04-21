using System;
using BattleEngine;

namespace RogueLibrary
{
    public class Rogue : Unit
    {
        public Rogue() : base("Rogue", 10, 4, 4, new Tuple<int, int>(2, 8), 8)
        {
            spells.Add(new Spells.PoisonStrike());
        }
    }
}
