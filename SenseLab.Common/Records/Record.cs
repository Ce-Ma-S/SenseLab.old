using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public abstract class Record :
        Locatable<ILocation>,
        IRecord
    {
        public string Text
        {
            get { return GetText(); }
        }

        protected abstract string GetText();

        private string text;
    }
}
