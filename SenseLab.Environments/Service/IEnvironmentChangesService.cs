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

        #endregion

        #region Recordables

        void RecordProvider_NextRecord(int id, IRecord record);

        #endregion
    }
}