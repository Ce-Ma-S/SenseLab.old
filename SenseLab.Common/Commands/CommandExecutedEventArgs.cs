using System;

namespace SenseLab.Common.Commands
{
    public class CommandExecutedEventArgs :
        EventArgs
    {
        public object Parameter { get; private set; }
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }
    }
}
