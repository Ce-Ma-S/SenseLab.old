using CeMaS.Common.Validation;

namespace CeMaS.Common.PhysicalQuantities
{
    public class Unit
    {
        public Unit(string name, string shortcut)
        {
            name.ValidateNonNullOrEmpty("name");
            shortcut.ValidateNonNullOrEmpty("shortcut");
            Name = name;
            Shortcut = shortcut;
        }

        public string Name { get; private set; }
        public string Shortcut { get; private set; }
    }
}
