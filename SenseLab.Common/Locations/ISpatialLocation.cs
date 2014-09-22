namespace SenseLab.Common.Locations
{
    public interface ISpatialLocation :
        ILocation
    {
        new ISpatialLocation Clone();
    }
}
