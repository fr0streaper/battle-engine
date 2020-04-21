using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class Effect
    {
        public BattleUnitsStack Owner { get; set; }

        public virtual void Modify(Statistics ally, Statistics enemy)
        { }

        public virtual bool IsValid()
        {
            return true;
        }

        public virtual void NewRound()
        { }
    
        public virtual void Action(object sender, ActionArgs e)
        {
            //Console.WriteLine("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        public virtual string ToReadableString()
        {
            return GetType().Name;
        }

        public virtual Effect Clone()
        {
            return this;
        }
    }
}
