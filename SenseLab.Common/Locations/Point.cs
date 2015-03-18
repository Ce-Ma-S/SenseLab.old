using System.Runtime.Serialization;
using System.Windows.Media.Media3D;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(OrientedPoint))]
    public class Point :
        SpatialLocation
    {
        public Point(Point3D position)
        {
            Position = position;
        }

        [DataMember]
        public Point3D Position
        {
            get { return position; }
            set
            {
                SetProperty(() => Position, ref position, value, OnChanged);
            }
        }

        public override string ToString()
        {
            return Position.ToString();
        }

        private Point3D position;
    }
}
