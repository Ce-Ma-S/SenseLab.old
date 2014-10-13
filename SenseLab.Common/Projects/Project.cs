using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Project.
    /// </summary>
    [DataContract]
    public class Project :
        ProjectNodeBase,
        IProject
    {
        public Project(Guid id, string name,
            IRecordStorage records)
            : base(id, name, null, null)
        {
            Records = records;
        }

        public IRecordStorage Records
        {
            get { return records; }
            set
            {
                SetProperty(() => Records, ref records, value,
                    beforeChange: (n, v) => v.ValidateNonNull(n));
            }
        }
        //public IList<IRecordTransformer> ReadRecordTransformers { get; private set; }
        //public IList<IRecordTransformer> WriteRecordTransformers { get; private set; }

        public async Task<IProject> Clone(Func<Guid, Task<IRecordStorage>> createRecords)
        {
            createRecords.ValidateNonNull("createRecords");
            var clone = (Project)Clone();
            clone.ClearEventHandlers();
            clone.Records = await createRecords(clone.Id);
            return clone;
        }

        private IRecordStorage records;
    }
}
