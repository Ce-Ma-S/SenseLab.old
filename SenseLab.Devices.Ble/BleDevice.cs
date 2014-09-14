namespace SenseLab.Common.ViewModels.Nodes.Devices.Ble
{
    /// <summary>
    /// Bluetooth Low Enegry (Smart) device.
    /// </summary>
    public abstract class BleDevice : Disposable
    {
        public abstract bool Connect();
    }
}
