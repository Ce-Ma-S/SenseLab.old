using CeMaS.Common.Validation;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    [DataContract]
    [KnownType(typeof(SpatialTextLocation))]
    public class TextLocation :
        Location
    {
        public TextLocation(string text)
        {
            Text = text;
        }

        [DataMember]
        public string Text
        {
            get { return text; }
            set
            {
                value.ValidateNonNullOrEmpty();
                SetProperty(() => Text, ref text, value, OnChanged);
            }
        }

        public override string ToString()
        {
            return Text;
        }

        private string text;
    }
}
