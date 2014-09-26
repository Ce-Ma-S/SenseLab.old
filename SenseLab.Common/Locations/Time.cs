using System;

namespace SenseLab.Common.Locations
{
    public class Time :
        Location,
        ITime
    {
        public Time(DateTimeOffset from, TimeSpan? length = null)
        {
            From = from;
            if (length.HasValue)
                length = TimeSpan.Zero;
            Length = length.Value;
        }
        public Time(DateTimeOffset from, DateTimeOffset to)
        {
            From = from;
            To = to;
        }

        public static Time Now { get { return new Time(DateTimeOffset.Now); } }

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

        public new ITemporalLocation Clone()
        {
            return (ITemporalLocation)base.Clone();
        }
        
        protected override string GetText()
        {
            return Length == TimeSpan.Zero ?
                From.ToString() :
                string.Format("{0} - {1} ({2})", From, To, Length);
        }
        protected virtual void OnFromChanged()
        {
            OnPropertyChanged(() => From);
            OnLengthChanged(false);
            OnChanged();
        }
        protected virtual void OnLengthChanged(bool onToChanged)
        {
            OnPropertyChanged(() => Length);
            if (onToChanged)
            {
                OnToChanged();
                OnChanged();
            }
        }
        protected virtual void OnToChanged()
        {
            OnPropertyChanged(() => To);
            OnChanged();
        }

        private DateTimeOffset from;
        private TimeSpan length;
    }
}
