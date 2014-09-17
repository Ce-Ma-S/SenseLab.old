namespace SenseLab.Common.Nodes
{
    public interface IDevice :
        IDeviceProvider
    {
        bool IsAvailable { get; }
    }
}
