using SenseLab.Common.Nodes;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public interface IRecorder
    {
        bool IsRecording { get;}
        IRecords CurrentRecords { get; }

        void Start(IEnumerable<INode> nodes, IRecords records);
        void Pause();
        void Stop();
    }
}
