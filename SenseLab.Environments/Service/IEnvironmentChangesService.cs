using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace SenseLab.Environments.Service
{
    public interface IEnvironmentChangesService
    {
        #region Environment

        [OperationContract(IsOneWay = true)]
        void Environment_NameChanged(string value);
        [OperationContract(IsOneWay = true)]
        void Environment_DescriptionChanged(string value);
        [OperationContract(IsOneWay = true)]
        void Environment_LocationChanged(ISpatialLocation value);
        [OperationContract(IsOneWay = true)]
        void Environment_DeviceProvidersAdded(IEnumerable<Guid> ids);
        [OperationContract(IsOneWay = true)]
        void Environment_DeviceProvidersRemoved(IEnumerable<Guid> ids);

        #endregion

        #region Device providers / Devices

        [OperationContract(IsOneWay = true)]
        void DeviceProvider_NameChanged(Guid deviceProviderId, string value);
        [OperationContract(IsOneWay = true)]
        void DeviceProvider_DescriptionChanged(Guid deviceProviderId, string value);
        [OperationContract(IsOneWay = true)]
        void DeviceProvider_DevicesAdded(Guid deviceProviderId, IEnumerable<Guid> ids);
        [OperationContract(IsOneWay = true)]
        void DeviceProvider_DevicesRemoved(Guid deviceProviderId, IEnumerable<Guid> ids);

        #endregion

        #region Devices

        [OperationContract(IsOneWay = true)]
        void Device_IsAvailableChanged(Guid deviceId, bool value);
        [OperationContract(IsOneWay = true)]
        void Device_IsConnectedChanged(Guid deviceId, bool value);
        [OperationContract(IsOneWay = true)]
        void Device_LocationChanged(Guid deviceId, ISpatialLocation value);
        [OperationContract(IsOneWay = true)]
        void Device_RecordablesAdded(Guid deviceId, IEnumerable<Guid> ids);
        [OperationContract(IsOneWay = true)]
        void Device_RecordablesRemoved(Guid deviceId, IEnumerable<Guid> ids);

        #endregion

        #region Recordables

        [OperationContract(IsOneWay = true)]
        void Recordable_NameChanged(Guid recordableId, string value);
        [OperationContract(IsOneWay = true)]
        void Recordable_DescriptionChanged(Guid recordableId, string value);
        [OperationContract(IsOneWay = true)]
        void Recordable_IsAvailableChanged(Guid recordableId, bool value);

        [OperationContract(IsOneWay = true)]
        void Command_CanExecuteChanged(Guid commandId);
        [OperationContract(IsOneWay = true)]
        void Command_Executed(Guid commandId, object parameter, DateTimeOffset start, DateTimeOffset end, bool canceled, Exception error);

        [OperationContract(IsOneWay = true)]
        [ServiceKnownType(typeof(Record))]
        [ServiceKnownType(typeof(TimeInterval))]
        [ServiceKnownType(typeof(SpatialLocation))]
        void RecordProvider_NextRecord(int id, IRecord record);

        #endregion
    }
}