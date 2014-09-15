using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public interface IRecord :
        ILocatable
    {
        string Text { get; }
        IRecorder Recorder { get; }
    }
}
