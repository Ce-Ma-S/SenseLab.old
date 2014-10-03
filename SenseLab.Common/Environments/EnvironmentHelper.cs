using Microsoft.Practices.ServiceLocation;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public static class EnvironmentHelper
    {
        public static IEnvironmentNode NodeFromId(Guid nodeId)
        {
            return (IEnvironmentNode)environment.FromId(nodeId);
        }
        public static IRecordable RecordableFromId(Guid recordableId)
        {
            return environment.AllChildren().Cast<IEnvironmentNode>().Select(n => n.RecordableFromId(recordableId)).FirstOrDefault();
        }
        public static IRecordable RecordableFromId(this IEnvironmentNode node, Guid recordableId)
        {
            return node.Recordables.FirstOrDefault(r => r.Id == recordableId);
        }
        public static IEnumerable<IRecordable> RecordablesFromIds(this IEnvironmentNode node, IEnumerable<Guid> recordableIds)
        {
            return node.Recordables.Where(r => recordableIds.Contains(r.Id));
        }

        private static readonly IEnvironment environment = ServiceLocator.Current.GetInstance<IEnvironment>();
    }
}
