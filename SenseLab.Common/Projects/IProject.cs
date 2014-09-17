using SenseLab.Common.Records;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    public interface IProject
    {
        string Name { get; }
        string Description { get; }
        
        IEnumerable<IProjectNode> Nodes { get; }
        IEnumerable<IRecords> Records { get; }
    }
}
