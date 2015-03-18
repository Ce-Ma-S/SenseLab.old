using CeMaS.Common.Validation;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    public class Uri :
        SpatialLocation
    {
        public Uri(System.Uri value)
        {
            value.ValidateNonNull(nameof(value));
            Value = value;
        }

        public System.Uri Value
        {
            get { return value; }
            set
            {
                SetProperty(() => Value, ref this.value, value, OnChanged);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        private System.Uri value;
    }
}
