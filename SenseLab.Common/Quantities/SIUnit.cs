using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.Quantities
{
    public class SIUnit : Unit
    {
        public SIUnit(string name, string shortcut, SIPrefix prefix = null)
            : base(name, shortcut)
        {
            Prefix = prefix;
        }

        public static readonly SIUnit Meter = new SIUnit("meter", "m");
        public static readonly SIUnit Volt = new SIUnit("Volt", "V");
        public static readonly SIUnit Ampere = new SIUnit("Ampere", "A");

        public SIPrefix Prefix { get; private set; }
    }
}
