using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    /// <summary>
    ///     Similar to an AND gate; it will only return a Success result if all child nodes run successfully.
    /// </summary>
    public class BtSequencer : BtNode
    {
        private readonly List<BtNode> _children;

        public BtSequencer(IEnumerable<BtNode> children)
        {
            _children = new List<BtNode>(children);
        }

        public override Result Execute(GameTime gameTime)
        {
            var isAnyNodeRunning = false;
            foreach (var node in _children)
                switch (node.Execute(gameTime))
                {
                    case Result.Running:
                        isAnyNodeRunning = true;
                        break;
                    case Result.Success:
                        break;
                    case Result.Failure:
                        return Result.Failure;
                }

            return isAnyNodeRunning ? Result.Running : Result.Success;
        }
    }
}