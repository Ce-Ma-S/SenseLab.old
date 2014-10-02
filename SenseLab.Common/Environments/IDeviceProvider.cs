using SenseLab.Common.Nodes;

namespace SenseLab.Common.Environments
{
    public interface IDeviceProvider :
        INode<IDevice>,
        IEnvironmentNode
    {
    }
}
