using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    /// <summary>
    ///     Similar to an OR gate; will return a result of Success if any of the child nodes returns Success.
    /// </summary>
    public class BtSelector : BtNode
    {
        private readonly List<BtNode> _children;

        public BtSelector(IEnumerable<BtNode> children)
        {
            _children = new List<BtNode>(children);
        }

        public override Result Execute(GameTime gameTime)
        {
            foreach (var node in _children)
                switch (node.Execute(gameTime))
                {
                    case Result.Running:
                        return Result.Running;
                    case Result.Failure:
                        break;
                    case Result.Success:
                        return Result.Success;
                }

            return Result.Failure;
        }
    }
}