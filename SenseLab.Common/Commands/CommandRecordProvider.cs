using CeMaS.Common.Commands;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Threading.Tasks;

namespace SenseLab.Common.Commands
{
    public class CommandRecordProvider<T> :
        RecordProvider
    {
        public CommandRecordProvider(IRecordableCommand command) :
            base(command)
        {
        }

        public IRecordableCommand Command
        {
            get { return (IRecordableCommand)Recordable; }
        }

        protected override async Task DoStart()
        {
            Command.Executed += OnExecuted;
            await Task.Yield();
        }
        protected override async Task DoStop()
        {
            Command.Executed -= OnExecuted;
            await Task.Yield();
        }
        protected override Task<IRecord> CreateRecord(object data)
        {
            var e = (CommandExecutedEventArgs)data;
            return Task.FromResult<IRecord>(
                new CommandRecord<T>(0, Command.Id, new TimeInterval(e.Start, e.End), (T)e.Parameter));
        }

        private async void OnExecuted(object sender, CommandExecutedEventArgs e)
        {
            if (!e.Canceled && e.Error == null)
                await AddRecord(e);
        }
    }
}
