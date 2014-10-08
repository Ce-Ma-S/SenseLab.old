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
            uint sequenceNumber,
            ITime temporalLocation,
            IRecordableCommand command,
            P commandParameter,
            ISpatialLocation spatialLocation = null)
            : base(command, sequenceNumber, temporalLocation, spatialLocation)
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
