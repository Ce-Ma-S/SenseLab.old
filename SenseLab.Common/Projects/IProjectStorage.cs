using SenseLab.Common.Data;
using SenseLab.Common.Records;
using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Projects
{
    public interface IProjectStorage :
        IItemStorage<IProject, Guid>
    {
        Task<IRecordStorage> CreateRecordStorage(Guid projectId);
    }
}
