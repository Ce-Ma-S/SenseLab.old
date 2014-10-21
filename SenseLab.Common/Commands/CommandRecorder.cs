using SenseLab.Common.Locations;
using SenseLab.Common.Records;

namespace SenseLab.Common.Commands
{
    public class CommandRecorder<T> :
        Recorder<CommandRecord<T>>
    {
        public CommandRecorder(IRecordableCommand command, uint nextSequenceNumber, ILocatable<ISpatialLocation> location) :
            base(command, nextSequenceNumber, location)
        {
        }

        public IRecordableCommand Command
        {
            get { return (IRecordableCommand)Recordable; }
        }

        protected override void DoStart()
        {
            Command.Executed += OnExecuted;
        }
        protected override void DoStop()
        {
            Command.Executed -= OnExecuted;
        }
        protected override CommandRecord<T> CreateRecord(object data, uint sequenceNumber, Locations.ISpatialLocation spatialLocation)
        {
            var e = (CommandExecutedEventArgs)data;
            return new CommandRecord<T>(Command.Id, sequenceNumber, new Time(e.Start, e.End), (T)e.Parameter, spatialLocation);
        }

        private void OnExecuted(object sender, CommandExecutedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
                AddRecord(e);
        }
    }
}
