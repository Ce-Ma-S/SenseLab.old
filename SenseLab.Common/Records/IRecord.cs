using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public interface IRecord :
        ILocatable<ISpatialLocation>,
        ILocatable<ITime>
    {
        string Text { get; }
        IRecordable Recordable { get; }
    }
}
