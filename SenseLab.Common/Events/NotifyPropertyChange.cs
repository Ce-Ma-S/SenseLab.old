using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SenseLab.Common.Events
{
    public abstract class NotifyPropertyChange : Disposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearEventHandlers();
            }
        }
        protected virtual void ClearEventHandlers()
        {
            PropertyChanged = null;
        }

        protected bool SetProperty<T>(Expression<Func<T>> property, ref T field, T value,
            Action<T, T> onValueChanged = null, EqualityComparer<T> valueComparer = null)
        {
            if (valueComparer == null)
            {
                valueComparer = EqualityComparer<T>.Default;
            }
            T oldValue = field;
            field = value;
            var changed = !valueComparer.Equals(oldValue, value);
            if (changed)
            {
                if (onValueChanged != null)
                    onValueChanged(oldValue, value);
                OnPropertyChanged(property);
            }
            return changed;
        }
        protected bool SetProperty<T>(Expression<Func<T>> property, ref T field, T value,
            Action onValueChanged = null, EqualityComparer<T> valueComparer = null)
        {
            return SetProperty(
                property, ref field, value,
                onValueChanged != null ? (o, n) => onValueChanged() : (Action<T, T>)null,
                valueComparer);
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            var propertyName = property.PropertyName();
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
