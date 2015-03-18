using System;

namespace CeMaS.Common
{
    /// <summary>
    /// Identifiable object.
    /// </summary>
    /// <typeparam name="T">Identifier type.</typeparam>
    public interface IIdWritable<T> :
        IId<T>
    {
        /// <summary>
        /// Identifier of this object.
        /// </summary>
        new T Id { get; set; }
        /// <summary>
        /// Fired when <see cref="Id"/> changes.
        /// </summary>
        event EventHandler IdChanged;
    }
}
