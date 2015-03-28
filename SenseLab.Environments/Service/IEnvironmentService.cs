using SenseLab.Common.Records;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SenseLab.Environments.Service
{
    [ServiceContract(
        CallbackContract = typeof(IEnvironmentChangesService),
        SessionMode = SessionMode.Required
        )]
    public interface IEnvironmentService
    {
        #region Environment

        [OperationContract]
        Task<EnvironmentInfo> Environment();

        #endregion

        #region Device providers

        [OperationContract]
        Task<DeviceProviderInfo> DeviceProvider(Guid id);

        #endregion

        #region Devices

        [OperationContract]
        Task<DeviceInfo> Device(Guid id);
        [OperationContract]
        Task Device_Connect(Guid id);
        [OperationContract]
        Task Device_Disconnect(Guid id);

        #endregion

        #region Recordables

        [OperationContract]
        Task<RecordSourceInfo> Recordable(Guid id);
        [OperationContract]
        Task<RecordProviderInfo> Recordable_CreateRecordProvider(Guid id);

        Task<bool> Command_CanExecute(Guid id, object parameter);
        Task Command_Execute(Guid id, object parameter);

        #endregion

        #region Record providers

        [OperationContract]
        Task<RecordProviderInfo> RecordProvider(int id);
        [OperationContract]
        Task RecordProvider_Dispose(int id);
        [OperationContract]
        Task RecordProvider_Start(int id);
        [OperationContract]
        Task RecordProvider_Pause(int id);
        [OperationContract]
        Task RecordProvider_Unpause(int id);
        [OperationContract]
        Task RecordProvider_Stop(int id);

        [OperationContract]
        Task<IRecord> SamplingRecordProvider_AddRecord(int id);
        [OperationContract]
        Task SamplingRecordProvider_SetRecordPeriodEnabled(int id, bool value);
        [OperationContract]
        Task SamplingRecordProvider_SetRecordPeriod(int id, TimeSpan value);

        [OperationContract]
        Task ValueRecordProvider_SetBatchSize(int id, int value);
        [OperationContract]
        Task ValueRecordProvider_SetBatchPeriod(int id, TimeSpan? value);

        [OperationContract]
        Task StreamingRecordProvider_SetStreamManagerId(int id, Guid value);

        #endregion

        #region Changes

        Task SubscribeToChanges();
        Task UnsubscribeFromChanges();

        #endregion
    }
}
