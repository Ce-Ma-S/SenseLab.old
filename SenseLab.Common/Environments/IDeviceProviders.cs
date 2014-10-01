using SenseLab.Common.Nodes;

namespace SenseLab.Common.Environments
{
    public interface IDeviceProviders
    {
        void Add(IDeviceProvider deviceProvider);
        bool Remove(IDeviceProvider deviceProvider);
    }
}
