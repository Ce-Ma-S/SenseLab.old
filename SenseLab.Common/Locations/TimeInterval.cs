using System;

namespace SenseLab.Common.Locations
{
    public class TimeInterval : Location, ITemporalLocation
    {
        public TimeInterval(DateTimeOffset from, TimeSpan? length = null)
        {
            From = from;
            if (length.HasValue)
                length = TimeSpan.Zero;
            Length = length.Value;
        }
        public TimeInterval(DateTimeOffset from, DateTimeOffset to)
        {
            From = from;
            To = to;
        }

        public static TimeInterval Now { get { return new TimeInterval(DateTimeOffset.Now); } }

        public DateTimeOffset From
        {
            get { return from; }
            set
            {
                SetProperty(() => From, ref from, value, OnFromChanged);
            }
        }
        public TimeSpan Length
        {
            get { return length; }
            set
            {
                SetProperty(() => Length, ref length, value, () => OnLengthChanged(true));
            }
        }
        public DateTimeOffset To
        {
            get { return From + Length; }
            set
            {
                SetProperty(() => To, v => Length = From - To, value);
            }
        }
        public override string Text
        {
            get
            {
                return Length == TimeSpan.Zero ?
                    From.ToString() :
                    string.Format("{0} - {1} ({2})", From, To, Length);
            }
        }

        protected virtual void OnFromChanged()
        {
            OnPropertyChanged(() => From);
            OnLengthChanged(false);
        }
        protected virtual void OnLengthChanged(bool onToChanged)
        {
            OnPropertyChanged(() => Length);
            if (onToChanged)
                OnToChanged();
        }
        protected virtual void OnToChanged()
        {
            OnPropertyChanged(() => To);
        }

        private DateTimeOffset from;
        private TimeSpan length;
    }
}
