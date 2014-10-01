using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Nodes
{
    public static class NodeHelper
    {
        public static IEnumerable<INode> AllChildren(this INode node)
        {
            foreach (var child in node.Children)
            {
                yield return child;
                foreach (var subChild in child.AllChildren())
                {
                    yield return subChild;
                }
            }
        }
        public static IEnumerable<INode> ThisAndAllChildren(this INode node)
        {
            yield return node;
            foreach (var child in node.AllChildren())
            {
                yield return child;
            }
        }
        public static INode FromId(this INode node, Guid id)
        {
            return node.ThisAndAllChildren().FirstOrDefault(n => n.Id == id);
        }
    }
}
