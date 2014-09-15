using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public interface IRecord :
        ILocatable<ISpatialLocation>,
        ILocatable<ITemporalLocation>
    {
        string Text { get; }
        IRecorder Recorder { get; }
    }
}
