using GrimGame.Game;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    public class BtRepeater : BtNode
    {
        private readonly BtNode _child;

        public BtRepeater(BtNode child)
        {
            _child = child;
        }

        public override Result Execute(GameTime gameTime)
        {
            GrimDebugger.Log($"Child returned {_child.Execute(gameTime)}");
            return Result.Running;
        }
    }
}