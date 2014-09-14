using System.Windows.Media.Media3D;

namespace SenseLab.Common.Locations
{
    public class Point :
        Location, ISpatialLocation
    {
        public Point3D Position
        {
            get { return position; }
            set
            {
                SetProperty(() => Position, ref position, value, OnTextChanged);
            }
        }

        protected override string GetText()
        {
            return Position.ToString();
        }

        private Point3D position;
    }
}
