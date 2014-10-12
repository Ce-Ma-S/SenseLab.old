using System;

namespace SenseLab.Common.Data
{
    public interface IChangeAware
    {
        bool IsChanged { get; set; }
        event EventHandler Changed;
    }
}
