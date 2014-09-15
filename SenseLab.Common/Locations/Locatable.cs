using SenseLab.Common.Events;

namespace SenseLab.Common.Locations
{
    public class Locatable<T> :
        NotifyPropertyChange,
        ILocatable
        where T : ILocation
    {
        public T Location
        {
            get { return location; }
            set
            {
                SetProperty(() => Location, ref location, value);
            }
        }
        ILocation ILocatable.Location
        {
            get { return Location; }
        }

        private T location;
    }
}
