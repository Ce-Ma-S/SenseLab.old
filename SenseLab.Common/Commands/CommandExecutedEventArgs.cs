using System;

namespace SenseLab.Common.Commands
{
    public class CommandExecutedEventArgs :
        EventArgs
    {
        public CommandExecutedEventArgs(object parameter, DateTimeOffset start, DateTimeOffset end, bool cancelled, Exception error)
        {
            Parameter = parameter;
            Start = start;
            End = end;
            Cancelled = cancelled;
            Error = error;
        }

        public object Parameter { get; private set; }
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }
        public bool Cancelled { get; private set; }
        public Exception Error { get; private set; }
    }
}
