using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;

namespace SenseLab.Common.Commands
{
    [DataContract]
    public class CommandRecord<T> :
        Record,
        ICommandRecord
    {
        public CommandRecord(
            uint id,
            Guid sourceId,
            ITimeInterval temporalLocation,
            T commandParameter,
            ISpatialLocation spatialLocation = null)
            : base(id, sourceId, temporalLocation, spatialLocation)
        {
            CommandParameter = commandParameter;
        }

        #region CommandParameter

        public T CommandParameter
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

        [DataMember(Name = "Parameter")]
        private T commandParameter;
        
        #endregion

        public override string ToString()
        {
            return string.Format("{0} ({1})", SourceId, CommandParameter);
        }
    }
}
