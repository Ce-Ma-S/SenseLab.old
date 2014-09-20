namespace SenseLab.Common.Environments
{
    public interface IDevice :
        IDeviceProvider
    {
        bool IsAvailable { get; }
    }
}
