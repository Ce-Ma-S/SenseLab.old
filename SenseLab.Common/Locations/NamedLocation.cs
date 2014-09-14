using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public class NamedLocation : Location, ISpatialLocation, ITemporalLocation
    {
        public NamedLocation(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return name; }
            set
            {
                value.ValidateNonNullOrEmpty();
                SetProperty(() => Name, ref name, value, OnNameChanged);
            }
        }
        public override string Text
        {
            get { return Name; }
        }

        protected virtual void OnNameChanged()
        {
            OnTextChanged();
        }

        private string name;
    }
}
