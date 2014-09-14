using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.Quantities
{
    public class SIPrefix
    {
        public SIPrefix(string name, double multiplier, string shortcut)
        {
            Name = name;
            Multiplier = multiplier;
            Shortcut = shortcut;
        }

        public static readonly SIPrefix Pico = new SIPrefix("pico", 1e-12, "p");
        public static readonly SIPrefix Nano = new SIPrefix("nano", 1e-9, "n");
        public static readonly SIPrefix Micro = new SIPrefix("micro", 1e-6, "μ");
        public static readonly SIPrefix Milli = new SIPrefix("milli", 1e-3, "m");
        public static readonly SIPrefix Centi = new SIPrefix("centi", 1e-2, "c");
        public static readonly SIPrefix Deci = new SIPrefix("deci", 1e-1, "d");

        public static readonly SIPrefix Deca = new SIPrefix("deca", 1e1, "da");
        public static readonly SIPrefix Hecto = new SIPrefix("hecto", 1e2, "h");
        public static readonly SIPrefix Kilo = new SIPrefix("kilo", 1e3, "k");
        public static readonly SIPrefix Mega = new SIPrefix("mega", 1e6, "M");
        public static readonly SIPrefix Giga = new SIPrefix("giga", 1e9, "G");
        public static readonly SIPrefix Tera = new SIPrefix("tera", 1e12, "T");

        public string Name { get; private set; }
        public double Multiplier { get; private set; }
        public string Shortcut { get; private set; }
    }
}
