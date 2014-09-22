namespace SenseLab.Common.Locations
{
    public interface ITemporalLocation :
        ILocation
    {
        new ITemporalLocation Clone();
    }
}
