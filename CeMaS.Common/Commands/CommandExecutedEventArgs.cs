using System;

namespace CeMaS.Common.Commands
{
    public class CommandExecutedEventArgs :
        EventArgs
    {
        public CommandExecutedEventArgs(object parameter, DateTimeOffset start, DateTimeOffset end, bool canceled, Exception error)
        {
            Parameter = parameter;
            Start = start;
            End = end;
            Canceled = canceled;
            Error = error;
        }

        public object Parameter { get; private set; }
        public DateTimeOffset Start { get; private set; }
        public DateTimeOffset End { get; private set; }
        public bool Canceled { get; private set; }
        public Exception Error { get; private set; }
    }
}
