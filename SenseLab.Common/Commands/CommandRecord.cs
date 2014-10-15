using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Commands
{
    [DataContract]
    public class CommandRecord<P> :
        Record,
        ICommandRecord
    {
        public CommandRecord(
            Guid sourceId,
            uint sequenceNumber,
            ITime temporalLocation,
            P commandParameter,
            ISpatialLocation spatialLocation = null)
            : base(sourceId, sequenceNumber, temporalLocation, spatialLocation)
        {
            CommandParameter = commandParameter;
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
            return string.Format("{0} ({1})", SourceId, CommandParameter);
        }

        [DataMember(Name = "Parameter")]
        private P commandParameter;
    }
}
