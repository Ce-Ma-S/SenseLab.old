using SenseLab.Common.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public class Command<T> :
        NotifyPropertyChange,
        ICommand
    {
        public Command(Action<T> execute, Func<T, bool> canExecute = null)
        {
            execute.ValidateNonNull("execute");
            IsSynchronous = true;
            executeTaskFactory = p => new Task(pp => execute((T)pp), p);
            canExecuteMethod = canExecute;
        }
        public Command(Func<T, Task> executeTaskFactory, Func<T, bool> canExecute = null, TaskScheduler taskScheduler = null)
        {
            executeTaskFactory.ValidateNonNull("executeTaskFactory");
            IsSynchronous = false;
            this.executeTaskFactory = executeTaskFactory;
            this.taskScheduler = taskScheduler ?? TaskScheduler.Default;
            canExecuteMethod = canExecute;
        }
        
        public bool IsSynchronous { get; private set; }
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter is T &&
                DoCanExecute((T)parameter);
        }
        public void Execute(object parameter)
        {
            T p = (T)parameter;
            var task = executeTaskFactory(p);
            var startData = OnExecuteStarted(p, task);
            if (IsSynchronous)
            {
                task.RunSynchronously();
                OnExecuteEnded(p, task, startData);
                return;
            }
            task.Start(taskScheduler);
            task.ContinueWith(t => OnExecuteEnded(p, t, startData), TaskScheduler.FromCurrentSynchronizationContext());
        }
        public void NotifyCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual bool DoCanExecute(T parameter)
        {
            return canExecuteMethod == null || canExecuteMethod(parameter);
        }
        protected virtual object OnExecuteStarted(T parameter, Task task)
        {
            return null;
        }
        protected virtual void OnExecuteEnded(T parameter, Task task, object startData)
        {
        }
        protected virtual void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
        
        private Func<T, bool> canExecuteMethod;
        private Func<T, Task> executeTaskFactory;
        private TaskScheduler taskScheduler;
    }


    public class Command :
        Command<object>
    {
        public Command(Action execute, Func<bool> canExecute = null)
            : base(
                p => execute(),
                canExecute == null ? (Func<object, bool>)null : p => canExecute())
        {
        }
        public Command(Func<Task> executeTaskFactory, Func<bool> canExecute = null, TaskScheduler taskScheduler = null)
            : base(
                p => executeTaskFactory(),
                canExecute == null ? (Func<object, bool>)null : p => canExecute(),
                taskScheduler)
        {
        }
    }
}
