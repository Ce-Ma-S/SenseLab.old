using System.Runtime.Serialization;
using System.Windows.Media.Media3D;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class OrientedPoint :
        Point
    {
        public OrientedPoint(Point3D position, Quaternion orientation)
            : base(position)
        {
            Orientation = orientation;
        }

        [DataMember]
        public Quaternion Orientation
        {
            get { return orientation; }
            set
            {
                SetProperty(() => Orientation, ref orientation, value, OnChanged);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}\n{1}", base.ToString(), Orientation.ToString());
        }

        private Quaternion orientation;
    }
}
