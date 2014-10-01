using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public static class EnvironmentHelper
    {
        public static IEnumerable<IRecordable> RecordablesFromIds(this IEnvironmentNode node, IEnumerable<Guid> recordableIds)
        {
            return node.Recordables.Where(r => recordableIds.Contains(r.Id));
        }
    }
}
