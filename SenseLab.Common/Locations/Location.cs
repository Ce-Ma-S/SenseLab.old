using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public abstract class Location : NotifyPropertyChange, ILocation
    {
        public abstract string Text
        {
            get;
        }

        public override string ToString()
        {
            return Text;
        }

        protected virtual void OnTextChanged()
        {
            OnPropertyChanged(() => Text);
        }
    }
}
