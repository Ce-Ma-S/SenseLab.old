using System;
using System.Threading.Tasks;

namespace CeMaS.Common
{
    public interface IConnectable
    {
        bool IsConnected { get; }
        event EventHandler IsConnectedChanged;

        Task Connect();
        Task Disconnect();
    }
}
