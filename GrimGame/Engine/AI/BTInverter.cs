using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    public class BtInverter : BtNode
    {
        private readonly BtNode _node;

        public BtInverter(BtNode node)
        {
            _node = node;
        }

        public override Result Execute(GameTime gameTime)
        {
            return _node.Execute(gameTime) switch
            {
                Result.Running => Result.Running,
                Result.Failure => Result.Success,
                Result.Success => Result.Failure,
                _ => Result.Failure
            };
        }
    }
}