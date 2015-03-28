using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CeMaS.Common.Commands
{
    public class Command<T> :
        NotifyPropertyChange,
        ICommandEx
    {
        public Command(Action<T, CancellationTokenSource> execute, bool isSynchronous, Func<T, bool> canExecute = null)
        {
            execute.ValidateNonNull(nameof(execute));
            IsSynchronous = isSynchronous;
            executeTaskFactory = (p, cts) => new Task(() => execute(p, cts), cts.Token);
            taskScheduler = TaskScheduler.Default;
            canExecuteMethod = canExecute;
        }
        public Command(Func<T, CancellationTokenSource, Task> executeTaskFactory, Func<T, bool> canExecute = null, TaskScheduler taskScheduler = null)
        {
            executeTaskFactory.ValidateNonNull(nameof(executeTaskFactory));
            IsSynchronous = false;
            this.executeTaskFactory = executeTaskFactory;
            this.taskScheduler = taskScheduler ?? TaskScheduler.Default;
            canExecuteMethod = canExecute;
        }
        
        public bool IsSynchronous { get; private set; }
        
        public event EventHandler CanExecuteChanged;
        public event EventHandler<CommandExecutedEventArgs> Executed;

        public bool CanExecute(object parameter)
        {
            return parameter is T &&
                DoCanExecute((T)parameter);
        }
        public void Execute(object parameter)
        {
            T p = (T)parameter;
            var cancellation = new CancellationTokenSource();
            var task = executeTaskFactory(p, cancellation);
            var startData = OnExecuteStarted(p, task);
            // synchronous
            if (IsSynchronous)
            {
                task.RunSynchronously();
                OnExecuteEnded(p, task, startData);
            }
            // asynchronous
            else
            {
                // start task if not running already
                if (task.Status == TaskStatus.Created)
                    task.Start(taskScheduler);
                task.ContinueWith(
                    t => OnExecuteEnded(p, t, startData),
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        public async Task NotifyCanExecuteChanged()
        {
            await new Action(OnCanExecuteChanged).CallOnDispatcherAsync();
        }

        protected virtual bool DoCanExecute(T parameter)
        {
            return canExecuteMethod == null || canExecuteMethod(parameter);
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged.RaiseEvent(this);
        }
        protected virtual object OnExecuteStarted(T parameter, Task task)
        {
            return DateTimeOffset.Now;
        }
        protected virtual void OnExecuteEnded(T parameter, Task task, object startData)
        {
            OnExecuted(
                parameter,
                (DateTimeOffset)startData,
                DateTimeOffset.Now,
                task.IsCanceled,
                task.Exception);
        }
        protected virtual void OnExecuted(T parameter, DateTimeOffset start, DateTimeOffset end, bool canceled, Exception error)
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

        private Func<T, bool> canExecuteMethod;
        private Func<T, CancellationTokenSource, Task> executeTaskFactory;
        private TaskScheduler taskScheduler;
    }


    public class Command :
        Command<object>
    {
        public Command(Action<CancellationTokenSource> execute, bool isSynchronous, Func<bool> canExecute = null)
            : base(
                (p, cts) => execute(cts), isSynchronous,
                canExecute == null ? (Func<object, bool>)null : p => canExecute())
        {
        }
        public Command(Func<CancellationTokenSource, Task> executeTaskFactory, Func<bool> canExecute = null, TaskScheduler taskScheduler = null)
            : base(
                (p, cts) => executeTaskFactory(cts),
                canExecute == null ? (Func<object, bool>)null : p => canExecute(),
                taskScheduler)
        {
        }
    }
}
