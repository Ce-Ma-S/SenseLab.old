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
        public IRecorder Recorder
        {
            get { return recorder; }
            set
            {
                SetProperty(() => Recorder, ref recorder, value);
            }
        }

        protected abstract string GetText();

        private IRecorder recorder;
    }
}
