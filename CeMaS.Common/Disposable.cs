using System;
using System.Runtime.Serialization;

namespace CeMaS.Common
{
    [DataContract]
    public abstract class Disposable :
        IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if (Disposed)
                return;
            Dispose(true);
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
