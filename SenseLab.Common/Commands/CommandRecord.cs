using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Runtime.Serialization;

namespace SenseLab.Common.Commands
{
    [DataContract]
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
            : base(command, sequenceNumber, spatialLocation, temporalLocation)
        {
            CommandParameter = commandParameter;
        }

        public IRecordableCommand Command
        {
            get { return (IRecordableCommand)Source; } 
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

        [DataMember(Name = "Parameter")]
        private P commandParameter;
    }
}
