using CeMaS.Common.Collections;
using SenseLab.Common.Locations;
using System;
using System.Collections.Generic;
using SenseLab.Common.Records;
using SenseLab.Common.Commands;
using CeMaS.Common.Commands;

namespace SenseLab.Environments.Local
{
    public abstract class Device :
        Common.Device
    {
        private Device(Environment environment,
            Guid id, string name, string description, ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            this.environment = environment;
        }

        #region IEnvironmentChangesService

        protected override void OnIsAvailableChanged()
        {
            base.OnIsAvailableChanged();
            environment.OnChange(service => service.Device_IsAvailableChanged(Id, IsAvailable));
        }
        protected override void OnIsConnectedChanged()
        {
            base.OnIsConnectedChanged();
            environment.OnChange(service => service.Device_IsConnectedChanged(Id, IsConnected));
        }
        protected override void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            base.OnLocationChanged(oldValue, newValue);
            environment.OnChange(service => service.Device_LocationChanged(Id, newValue));
        }
        protected override void OnLocationChanged(ISpatialLocation location)
        {
            base.OnLocationChanged(location);
            environment.OnChange(service => service.Environment_LocationChanged(location));
        }
        protected override void OnDevicesAdded(IEnumerable<IDevice> items)
        {
            base.OnDevicesAdded(items);
            environment.OnChange(service => service.DeviceProvider_DevicesAdded(Id, items.Ids()));
        }
        protected override void OnDevicesRemoved(IEnumerable<IDevice> items)
        {
            base.OnDevicesRemoved(items);
            environment.OnChange(service => service.DeviceProvider_DevicesRemoved(Id, items.Ids()));
        }
        protected override void OnRecordablesAdded(IEnumerable<IRecordable> items)
        {
            base.OnRecordablesAdded(items);
            RegisterEventHandlers(true, items);
            environment.OnChange(service => service.Device_RecordablesAdded(Id, items.Ids()));
        }
        protected override void OnRecordablesRemoved(IEnumerable<IRecordable> items)
        {
            base.OnRecordablesRemoved(items);
            RegisterEventHandlers(false, items);
            environment.OnChange(service => service.Device_RecordablesRemoved(Id, items.Ids()));
        }

        protected virtual void OnRecordableNameChanged(object sender, EventArgs e)
        {
            var recordable = (IRecordable)sender;
            environment.OnChange(service => service.Recordable_NameChanged(recordable.Id, recordable.Name));
        }
        protected virtual void OnRecordableDescriptionChanged(object sender, EventArgs e)
        {
            var recordable = (IRecordable)sender;
            environment.OnChange(service => service.Recordable_DescriptionChanged(recordable.Id, recordable.Description));
        }
        protected virtual void OnRecordableIsAvailableChanged(object sender, EventArgs e)
        {
            var recordable = (IRecordable)sender;
            environment.OnChange(service => service.Recordable_IsAvailableChanged(recordable.Id, recordable.IsAvailable));
        }
        protected virtual void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            var command = (IRecordableCommand)sender;
            environment.OnChange(service => service.Command_CanExecuteChanged(command.Id));
        }
        protected virtual void OnCommandExecuted(object sender, CommandExecutedEventArgs e)
        {
            var command = (IRecordableCommand)sender;
            environment.OnChange(service => service.Command_Executed(command.Id, e.Parameter, e.Start, e.End, e.Canceled, e.Error));
        }

        private void RegisterEventHandlers(bool register, IEnumerable<IRecordable> items)
        {
            if (register)
            {
                foreach (var recordable in items)
                {
                    recordable.NameChanged += OnRecordableNameChanged;
                    recordable.DescriptionChanged += OnRecordableDescriptionChanged;
                    recordable.IsAvailableChanged += OnRecordableIsAvailableChanged;
                    if (recordable is IRecordableCommand)
                    {
                        var command = (IRecordableCommand)recordable;
                        command.CanExecuteChanged += OnCommandCanExecuteChanged;
                        command.Executed += OnCommandExecuted;
                    }
                }
            }
            else
            {
                foreach (var recordable in items)
                {
                    recordable.NameChanged -= OnRecordableNameChanged;
                    recordable.DescriptionChanged -= OnRecordableDescriptionChanged;
                    recordable.IsAvailableChanged -= OnRecordableIsAvailableChanged;
                    if (recordable is IRecordableCommand)
                    {
                        var command = (IRecordableCommand)recordable;
                        command.CanExecuteChanged -= OnCommandCanExecuteChanged;
                        command.Executed -= OnCommandExecuted;
                    }
                }
            }
        }

        private readonly Environment environment;

        #endregion

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            RegisterEventHandlers(false, Recordables);
        }
    }
}