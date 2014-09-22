namespace SenseLab.Common.Locations
{
    public class TextLocation :
        Location
    {
        public TextLocation(string text)
        {
            Text = text;
        }

        public new string Text
        {
            get { return text; }
            set
            {
                value.ValidateNonNullOrEmpty();
                SetProperty(() => Text, ref text, value, OnChanged);
            }
        }

        protected override string GetText()
        {
            return Text;
        }

        private string text;
    }
}
