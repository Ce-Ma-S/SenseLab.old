using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.Quantities
{
    public class Unit
    {
        public Unit(string name, string shortcut)
        {
            Name = name;
            Shortcut = shortcut;
        }

        public string Name { get; private set; }
        public string Shortcut { get; private set; }
    }
}
