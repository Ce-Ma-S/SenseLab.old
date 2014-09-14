using System.Windows.Media.Media3D;

namespace SenseLab.Common.Locations
{
    public class OrientedPoint : Point
    {
        public Quaternion Orientation
        {
            get { return orientation; }
            set
            {
                SetProperty(() => Orientation, ref orientation, value, OnTextChanged);
            }
        }

        public override string Text
        {
            get { return string.Format("{0}\n{1}", base.Text, Orientation.ToString()); }
        }

        private Quaternion orientation;
    }
}
