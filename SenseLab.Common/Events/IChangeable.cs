using System;

namespace SenseLab.Common.Events
{
    /// <summary>
    /// Allows an object to notify its internal change.
    /// </summary>
    public interface IChangeable
    {
        /// <summary>
        /// Fired when an object is changed internally.
        /// </summary>
        event EventHandler Changed;
    }
}
