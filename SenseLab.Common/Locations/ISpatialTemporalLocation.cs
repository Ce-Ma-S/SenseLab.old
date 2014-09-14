namespace SenseLab.Common.Locations
{
    public interface ISpatialTemporalLocation :
        ISpatialLocation, ILocatable<ISpatialLocation>,
        ITemporalLocation, ILocatable<ITemporalLocation>,
        ILocationGroup
    {
    }
}
