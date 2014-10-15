using SenseLab.Common.Collections;
using SenseLab.Common.Data;
using SenseLab.Common.Events;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Project.
    /// </summary>
    [DataContract]
    public class Project :
        ProjectNodeBase,
        IProject
    {
        public Project(Guid id, string name,
            IRecordStorage records)
            : base(id, name, null, null)
        {
            Records = records;
            recordGroups = new ObservableCollectionEx<IRecordGroup, Guid>();
        }

        #region Records

        public IRecordStorage Records
        {
            get { return records; }
            set
            {
                try
                {
                    SetIsChangedOnChanged = false;
                    SetProperty(() => Records, ref records, value,
                        beforeChange: (n, v) => v.ValidateNonNull(n));
                }
                finally
                {
                    SetIsChangedOnChanged = true;
                }
            }
        }

        //public IList<IRecordTransformer> ReadRecordTransformers { get; private set; }
        //public IList<IRecordTransformer> WriteRecordTransformers { get; private set; }

        private IRecordStorage records;

        #endregion

        #region RecordGroups

        public IList<IRecordGroup> RecordGroups
        {
            get { return recordGroups; }
        }

        [DataMember(Name = "RecordGroups")]
        private IEnumerable<IRecordGroup> RecordGroupsSerialized
        {
            get { return recordGroups; }
            set
            {
                recordGroups = new ObservableCollectionEx<IRecordGroup, Guid>(value);
            }
        }

        private ObservableCollectionEx<IRecordGroup, Guid> recordGroups;

        #endregion

        #region Recording

        public bool IsRecording
        {
            get { return recordableToRecorder != null; }
        }
        public bool IsRecordingPaused
        {
            get { return isRecordingPaused; }
            set
            {
                SetProperty(() => IsRecordingPaused, ref isRecordingPaused, value, OnIsRecordingPausedChanged);
            }
        }

        public bool CanStartRecording(IRecordGroup group)
        {
            return Records.IsWritableAndConnected() && !IsRecording &&
                RecordGroups != null && RecordGroups.Contains(group);
        }
        public void StartRecording(IRecordGroup group)
        {
            if (IsRecording)
                throw new InvalidOperationException("Recording is already in progress.");
            recordableToRecorder = new Dictionary<IRecordable, IRecorder>();
            recordGroup = group;
            OnIsRecordingChanged();
            ProcessNodesForRecording(true, Children);
        }
        public bool CanStopRecording
        {
            get { return IsRecording; }
        }
        public void StopRecording(IRecordGroup group)
        {
            if (!IsRecording)
                throw new InvalidOperationException("Recording is not in progress.");
            ProcessNodesForRecording(false, Children);
            recordableToRecorder = null;
            recordGroup = null;
            OnIsRecordingChanged();
        }

        protected override void OnChildrenChanged(ValueChangeEventArgs<IEnumerable<ProjectNode>> e)
        {
            base.OnChildrenChanged(e);
            if (IsRecording)
            {
                ProcessNodesForRecording(false, e.OldValue.Value);
                ProcessNodesForRecording(true, e.NewValue);
            }
        }

        private void ProcessNodesForRecording(bool start, IEnumerable<IProjectNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (start)
                {
                    if (node.IsEnabled)
                        ProcessRecordables(true, node.EnabledRecordables);
                    node.IsEnabledChanged += OnIsEnabledChanged;
                    ((INode)node).Children.ItemContainmentChanged += OnChildrenChanged;
                }
                else
                {
                    if (node.IsEnabled)
                        ProcessRecordables(false, node.EnabledRecordables);
                    node.IsEnabledChanged -= OnIsEnabledChanged;
                    ((INode)node).Children.ItemContainmentChanged -= OnChildrenChanged;
                }
                ProcessNodesForRecording(start, node.Children);
            }
        }
        private void ProcessRecordables(bool start, INotifyEnumerable<IRecordable> recordables)
        {
            ProcessRecordables(start, (IEnumerable<IRecordable>)recordables);
            if (start)
                recordables.ItemContainmentChanged += OnEnabledRecordablesChanged;
            else
                recordables.ItemContainmentChanged -= OnEnabledRecordablesChanged;
        }
        private void ProcessRecordables(bool start, IEnumerable<IRecordable> recordables)
        {
            if (start)
            {
                foreach (var recordable in recordables)
                    StartRecording(recordable);
            }
            else
            {
                foreach (var recordable in recordables)
                    StopRecording(recordable);
            }
        }
        private void StartRecording(IRecordable recordable)
        {
            // TODO: find nextSequenceNumber
            uint nextSequenceNumber = 0;
            var recorder = recordable.CreateRecorder(recordGroup, nextSequenceNumber, this);
            recordableToRecorder.Add(recordable, recorder);
            recorder.IsPaused = isRecordingPaused;
            recorder.Do(async record => await Records.Save(record));
        }
        private void StopRecording(IRecordable recordable)
        {
            var recorder = recordableToRecorder[recordable];
            recorder.Dispose();
            recordableToRecorder.Remove(recordable);
        }

        private void OnIsRecordingChanged()
        {
            OnPropertyChanged(() => IsRecording);
        }
        private void OnIsRecordingPausedChanged()
        {
            if (!IsRecording)
                return;
            foreach (var recorder in recordableToRecorder.Values)
                recorder.IsPaused = isRecordingPaused;
        }
        private void OnIsEnabledChanged(object sender, EventArgs e)
        {
            var node = (IProjectNode)sender;
            ProcessRecordables(node.IsEnabled, node.EnabledRecordables);
        }
        private void OnChildrenChanged(object sender, ValueChangeEventArgs<IEnumerable<INode>> e)
        {
            ProcessNodesForRecording(false, e.OldValue.Value.Cast<IProjectNode>());
            ProcessNodesForRecording(true, e.NewValue.Cast<IProjectNode>());
        }
        private void OnEnabledRecordablesChanged(object sender, ValueChangeEventArgs<IEnumerable<IRecordable>> e)
        {
            ProcessRecordables(false, e.OldValue.Value);
            ProcessRecordables(true, e.NewValue);
        }

        private IRecordGroup recordGroup;
        private bool isRecordingPaused;
        private Dictionary<IRecordable, IRecorder> recordableToRecorder;

        #endregion

        #region Clone

        public async Task<IProject> Clone(Func<Guid, Task<IRecordStorage>> createRecords)
        {
            createRecords.ValidateNonNull("createRecords");
            var clone = (Project)Clone();
            clone.ClearEventHandlers();
            clone.recordGroups = new ObservableCollectionEx<IRecordGroup, Guid>(TryClone(recordGroups));
            clone.Records = await createRecords(clone.Id);
            return clone;
        }

        private IEnumerable<IRecordGroup> TryClone(IEnumerable<IRecordGroup> recordGroups)
        {
            return recordGroups.Select(recordGroup => recordGroup is RecordGroup ? ((RecordGroup)recordGroup).Clone() : recordGroup);
        }

        #endregion
    }
}
