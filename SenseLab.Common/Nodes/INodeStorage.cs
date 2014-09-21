using System;

namespace SenseLab.Common.Nodes
{
    /// <summary>
    /// Storage of items.
    /// Local or remote.
    /// </summary>
    public interface INodeStorage<T> :
        INode<INode, T>
        where T : INode
    {
        bool IsReadOnly { get; }
        bool IsConnected { get; }

        void Connect();
        void Disconnect();

        OptionalValue<T> Get(Guid itemId);

        void Add(T item);
        void Remove(Guid itemId);
    }
}
