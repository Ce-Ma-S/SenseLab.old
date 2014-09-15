namespace SenseLab.Common.Locations
{
    public interface ISpatialTemporalLocation :
        ILocationGroup
    {
        ISpatialLocation SpatialLocation { get; }
        ITemporalLocation TemporalLocation { get; }
    }
}
