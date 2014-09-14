using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public abstract class Location : NotifyPropertyChange, ILocation
    {
        public string Text
        {
            get { return GetText(); }
        }

        public override string ToString()
        {
            return Text;
        }

        protected abstract string GetText();
        protected virtual void OnTextChanged()
        {
            OnPropertyChanged(() => Text);
        }
    }
}
