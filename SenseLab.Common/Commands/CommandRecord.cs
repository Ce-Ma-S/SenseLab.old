using SenseLab.Common.Records;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public class CommandRecord<P> :
        Record,
        ICommandRecord
    {
        public ICommand Command
        {
            get { return command; }
            set
            {
                SetProperty(() => Command, ref command, value);
            }
        }
        public P CommandParameter
        {
            get { return commandParameter; }
            set
            {
                SetProperty(() => CommandParameter, ref commandParameter, value);
            }
        }

        protected override string GetText()
        {
            return string.Format("{0} ({1})", Command, CommandParameter);
        }
        object ICommandRecord.CommandParameter
        {
            get { return CommandParameter; }
        }

        private ICommand command;
        private P commandParameter;
    }
}
