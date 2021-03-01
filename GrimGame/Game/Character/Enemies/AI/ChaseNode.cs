using GrimGame.Engine;
using GrimGame.Engine.AI;
using Microsoft.Xna.Framework;

namespace GrimGame.Game.Character.AI
{
    public class ChaseNode : BtNode
    {
        private readonly GameObject _target;
        private readonly Enemy      _enemy;

        public ChaseNode(GameObject target, Enemy enemy)
        {
            _target = target;
            _enemy = enemy;
        }

        public override Result Execute(GameTime gameTime)
        {
            _enemy.MoveTowards(_target.Position);

            // If the enemy is still moving to position, return running, else, return Success
            return _enemy.DistanceToDestination > 0.2f ? Result.Running : Result.Success;
        }
    }
}