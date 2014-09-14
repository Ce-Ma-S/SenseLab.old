namespace SenseLab.Common.PhysicalQuantities
{
    public class PhysicalQuantity
    {
        public PhysicalQuantity(string name, string description = null)
        {
            name.ValidateNonNullOrEmpty("name");
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
