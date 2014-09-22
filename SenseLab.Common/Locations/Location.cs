using SenseLab.Common.Events;
using System;

namespace SenseLab.Common.Locations
{
    public abstract class Location :
        NotifyPropertyChange,
        ILocation,
        IChangeable
    {
        public string Text
        {
            get { return GetText(); }
        }

        public event EventHandler Changed;

        public virtual ILocation Clone()
        {
            var clone = (Location)MemberwiseClone();
            clone.ClearEventHandlers();
            return clone;
        }
        public override string ToString()
        {
            return Text;
        }

        protected abstract string GetText();

        protected virtual void OnChanged()
        {
            OnPropertyChanged(() => Text);
        }
    }
}
