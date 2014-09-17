namespace SenseLab.Common.Nodes
{
    public interface IDevice :
        IDeviceNode
    {
        bool IsAvailable { get; }
    }
}
