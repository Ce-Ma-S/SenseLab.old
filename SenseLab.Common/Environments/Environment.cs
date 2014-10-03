using SenseLab.Common.Nodes;
using System;
using System.ComponentModel.Composition;

namespace SenseLab.Common.Environments
{
    [Export(typeof(IEnvironment))]
    [Export(typeof(IDeviceProviders))]
    internal class Environment :
        NodeWritable<IEnvironmentNode>,
        IEnvironment,
        IDeviceProviders
    {
        public static readonly Environment Instance = new Environment(
            new Guid("3E57C422-F6FD-4742-8465-9F4633390765"),
            "Environment",
            "Provides access to devices");

        protected Environment(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }

        public void Add(IDeviceProvider deviceProvider)
        {
            deviceProvider.ValidateNonNull("deviceProvider");
            Children.Add(deviceProvider);
        }
        public bool Remove(IDeviceProvider deviceProvider)
        {
            return Children.Remove(deviceProvider);
        }
    }
}
