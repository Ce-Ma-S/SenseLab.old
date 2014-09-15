using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    public interface IProject
    {
        IEnumerable<INode> Nodes { get; }
        IEnumerable<IRecords> Records { get; }
    }
}
