using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleEngine
{
    public class ActionArgs : EventArgs
    {
        public string ID { get; }
        public BattleUnitsStack Actor { get; set; }
        public List<BattleUnitsStack> Targets { get; set; }
        public object Misc { get; set; }

        public ActionArgs(string id)
        {
            ID = id;
        }
    }
}
