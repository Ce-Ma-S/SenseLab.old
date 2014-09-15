namespace SenseLab.Common.Nodes
{
    public interface IDevice :
        INode
    {
        bool IsAvailable { get; }
    }
}
