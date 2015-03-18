using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Values
{
    [DataContract]
    public class ValuesRecord<T> :
        Record,
        IValuesRecord<T>
    {
        public ValuesRecord(
            uint id,
            Guid sourceId,
            IEnumerable<ILocatableValue<T>> values,
            ITimeInterval temporalLocation = null,
            ISpatialLocation spatialLocation = null)
            : base(id, sourceId, temporalLocation == null ? GetTemporalLocation(values) : temporalLocation, spatialLocation)
        {
            Values = new ObservableCollectionEx<ILocatableValue<T>>(values);
        }

        protected virtual object OnValuesChanged()
        {
            throw new NotImplementedException();
        }

        [DataMember]
        public INotifyList<ILocatableValue<T>> Values { get; private set; }
        IEnumerable<ILocatableValue<T>> IValuesRecord<T>.Values
        {
            get { return Values; }
        }
        IEnumerable<ILocatableValue> IValuesRecord.Values
        {
            get { return Values; }
        }
        public bool UpdateTemporalLocationFromValues
        {
            get { return updateTemporalLocationFromValues != null; }
            set
            {
                SetProperty(() => UpdateTemporalLocationFromValues,
                    v =>
                    {
                        if (v)
                        {
                            if (updateTemporalLocationFromValues == null)
                            {
                                DoUpdateTemporalLocationFromValues();
                                updateTemporalLocationFromValues = Values.Changed().Subscribe(_ => DoUpdateTemporalLocationFromValues());
                            }
                        }
                        else
                        {
                            DisposeUpdateTemporalLocationFromValues();
                        }
                    },
                    value);
            }
        }

        public override string ToString()
        {
            return string.Join("\n", Values);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                DisposeUpdateTemporalLocationFromValues();
        }

        private static ITimeInterval GetTemporalLocation(IEnumerable<ILocatableValue<T>> values)
        {
            if (!values.Any())
                return TimeInterval.Now;
            return values.
                Select(v => ((ILocatable<ITimeInterval>)v).Location).
                Where(t => t != null).
                Aggregate((t1, t2) => t1.Including(t2));
        }

        private void DoUpdateTemporalLocationFromValues()
        {
            TemporalLocation = GetTemporalLocation(Values);
        }
        private void DisposeUpdateTemporalLocationFromValues()
        {
            if (updateTemporalLocationFromValues != null)
            {
                updateTemporalLocationFromValues.Dispose();
                updateTemporalLocationFromValues = null;
            }
        }

        private IDisposable updateTemporalLocationFromValues;
    }
}
