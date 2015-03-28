using SenseLab.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CeMaS.Common.Commands;
using SenseLab.Common.Records;
using SenseLab.Environments.Service;
using CeMaS.Common.Events;

namespace SenseLab.Environments.Remote
{
    public class RecordableCommand :
        Recordable,
        IRecordableCommand
    {
        internal RecordableCommand(Environment environment, IEnvironmentService service,
            Guid id, IRecordType type, string name, string description, bool isAvailable,
            bool isSynchronous)
            : base(environment, service,
                  id, type, name, description, isAvailable)
        {
            IsSynchronous = isSynchronous;
        }

        public bool IsSynchronous { get; private set; }
        public event EventHandler CanExecuteChanged;
        public event EventHandler<CommandExecutedEventArgs> Executed;

        public bool CanExecute(object parameter)
        {
            return Service.Command_CanExecute(Id, parameter).Result;
        }
        public void Execute(object parameter)
        {
            Service.Command_Execute(Id, parameter).Wait();
        }

        internal virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged.RaiseEvent(this);
        }
        internal virtual void OnExecuted(object parameter, DateTimeOffset start, DateTimeOffset end, bool canceled, Exception error)
        {
            Executed.RaiseEvent(
                this,
                () => new CommandExecutedEventArgs(
                    parameter,
                    start,
                    end,
                    canceled,
                    error));
        }
    }
}
