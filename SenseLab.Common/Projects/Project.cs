using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Runtime.Serialization;

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
        public Project(Guid id, string name, string description = null,
            //INode parent = null,
            ISpatialLocation location = null)
            : base(id, name, description, /*parent,*/ location)
        {
        }

        [DataMember]
        public IRecordStorage Records { get; private set; }
        //public IList<IRecordTransformer> ReadRecordTransformers { get; private set; }
        //public IList<IRecordTransformer> WriteRecordTransformers { get; private set; }
    }
}
