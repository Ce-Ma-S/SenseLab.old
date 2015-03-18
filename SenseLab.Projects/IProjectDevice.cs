using CeMaS.Common.Collections;
using SenseLab.Common.Records;
using SenseLab.Environments;
using System;

namespace SenseLab.Projects
{
    /// <summary>
    /// Device in a project.
    /// </summary>
    public interface IProjectDevice
    {
        /// <summary>
        /// Device.
        /// </summary>
        IDevice Device { get; }
        /// <summary>
        /// Whether <see cref="Device"/> is enabled and its <see cref="Recordables"/> should be used.
        /// </summary>
        bool IsEnabled { get; }
        /// <summary>
        /// Fired when <see cref="IsEnabled"/> changes.
        /// </summary>
        event EventHandler IsEnabledChanged;
        /// <summary>
        /// <see cref="Device"/>`s recordables to be used in a project.
        /// </summary>
        INotifyList<IRecordable> Recordables { get; }
    }
}
