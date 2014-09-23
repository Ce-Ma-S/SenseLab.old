using SenseLab.Common.Locations;

namespace SenseLab.Common.Records
{
    public class PreviewRecorder :
        Recorder<IRecord>
    {
        public PreviewRecorder(IRecordable recordable, IRecords records, ILocatable<ISpatialLocation> location)
            : base(records, location)
        {
            recordable.ValidateNonNull("recordable");
            this.recordable = recordable;
        }

        public override IRecordable Recordable
        {
            get { return recordable; }
        }
        public IRecorder Recorder { get; private set; }

        protected override void DoStart()
        {
            Recorder = Recordable.CreateRecorder(Records, Location);
            Recorder.Start();
        }
        protected override void DoStop()
        {
            Recorder.Stop();
            Recorder.Dispose();
            Recorder = null;
        }
        protected override void DoPause()
        {
        }
        protected override void DoUnpause()
        {
        }
        protected override IRecord CreateRecord(object data, ISpatialLocation spatialLocation)
        {
            return null;
        }

        protected override void OnIsPausedChanged()
        {
            Recorder.IsPaused = IsPaused;
        }

        private IRecordable recordable;
    }
}
