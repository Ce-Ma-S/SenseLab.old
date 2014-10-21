using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public class RecordableCommand<T> :
        Command<T>,
        IRecordableCommand
    {
        public RecordableCommand(Guid id, string name, Action<T> execute,
            string description = null, Func<T, bool> canExecute = null,
            Func<bool> isAvailable = null) :
            base(execute, canExecute)
        {
            name.ValidateNonNullOrEmpty("name");
            Id = id;
            Name = name;
            Description = description;
            isAvailableMethod = isAvailable;
        }
        public RecordableCommand(Guid id, string name, Func<T, Task> executeTaskFactory,
            string description = null, Func<T, bool> canExecute = null, TaskScheduler taskScheduler = null,
            Func<bool> isAvailable = null) :
            base(executeTaskFactory, canExecute, taskScheduler)
        {
            name.ValidateNonNullOrEmpty("name");
            Id = id;
            Name = name;
            Description = description;
            isAvailableMethod = isAvailable;
        }

        public static readonly IRecordType RecordType = new RecordType(new Guid("0FCF04EB-302C-4149-A7AF-D4C691C40C94"), "Command", "Command execution");

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IRecordType Type
        {
            get { return RecordType; }
        }
        public virtual bool IsAvailable
        {
            get { return isAvailableMethod == null || isAvailableMethod(); }
        }

        public event EventHandler<CommandExecutedEventArgs> Executed;

        public IRecorder CreateRecorder(IRecordGroup group = null, uint nextSequenceNumber = 0, ILocatable<ISpatialLocation> location = null)
        {
            return new CommandRecorder<T>(this, nextSequenceNumber, location);
        }
        public void OnIsAvailableChanged()
        {
            OnPropertyChanged(() => IsAvailable);
        }

        protected override object OnExecuteStarted(T parameter, Task task)
        {
            base.OnExecuteStarted(parameter, task);
            return DateTimeOffset.Now;
        }
        protected override void OnExecuteEnded(T parameter, Task task, object startData)
        {
            base.OnExecuteEnded(parameter, task, startData);
            OnExecuted(new CommandExecutedEventArgs(
                parameter,
                (DateTimeOffset)startData,
                DateTimeOffset.Now,
                task.IsCanceled,
                task.Exception));
        }
        protected virtual void OnExecuted(CommandExecutedEventArgs a)
        {
            if (Executed != null)
                Executed(this, a);
        }

        private Func<bool> isAvailableMethod;
    }


    public class RecordableCommand :
        RecordableCommand<object>
    {
        public RecordableCommand(Guid id, string name, Action execute,
            string description = null, Func<bool> canExecute = null,
            Func<bool> isAvailable = null) :
            base(
                id, name,
                p => execute(),
                description,
                canExecute == null ? (Func<object, bool>)null : p => canExecute(),
                isAvailable)
        {
        }
        public RecordableCommand(Guid id, string name, Func<Task> executeTaskFactory,
            string description = null, Func<bool> canExecute = null, TaskScheduler taskScheduler = null,
            Func<bool> isAvailable = null) :
            base(
                id, name,
                p => executeTaskFactory(),
                description,
                canExecute == null ? (Func<object, bool>)null : p => canExecute(),
                taskScheduler,
                isAvailable)
        {
        }
    }
}
