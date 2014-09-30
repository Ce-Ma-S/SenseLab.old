using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public class CommandRecord<P> :
        Record,
        ICommandRecord
    {
        public CommandRecord(
            IRecordableCommand command,
            P commandParameter,
            uint sequenceNumber,
            ISpatialLocation spatialLocation = null,
            ITime temporalLocation = null)
            : base(sequenceNumber, spatialLocation, temporalLocation)
        {
            command.ValidateNonNull("command");
            Command = command;
            CommandParameter = commandParameter;
        }

        public IRecordableCommand Command { get; private set; }
        public override IRecordSource Source
        {
            get { return Command; }
        }
        public P CommandParameter
        {
            get { return commandParameter; }
            set
            {
                SetProperty(() => CommandParameter, ref commandParameter, value);
            }
        }
        object ICommandRecord.CommandParameter
        {
            get { return CommandParameter; }
        }

        protected override string GetText()
        {
            return string.Format("{0} ({1})", Command, CommandParameter);
        }

        private P commandParameter;
    }
}
