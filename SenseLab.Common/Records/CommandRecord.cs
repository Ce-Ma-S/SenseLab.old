using System.Windows.Input;

namespace SenseLab.Common.Records
{
    public class CommandRecord<T> :
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
        public T CommandParameter
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
        private T commandParameter;
    }
}
