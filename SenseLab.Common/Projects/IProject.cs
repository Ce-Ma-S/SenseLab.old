using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    public interface IProject
    {
        IEnumerable<IProjectNode> Nodes { get; }
        IEnumerable<IRecords> Records { get; }
    }
}
