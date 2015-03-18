using System;

namespace CeMaS.Common
{
    /// <summary>
    /// Item with basic information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemInfo<T> :
        IId<T>
    {
        /// <summary>
        /// Name.
        /// </summary>
        /// <value>non-empty</value>
        string Name { get; }
        /// <summary>
        /// Fired when <see cref="Name"/> changes.
        /// </summary>
        event EventHandler NameChanged;
        /// <summary>
        /// Description.
        /// </summary>
        /// <value>optional</value>
        string Description { get; }
        /// <summary>
        /// Fired when <see cref="Description"/> changes.
        /// </summary>
        event EventHandler DescriptionChanged;
    }
}
