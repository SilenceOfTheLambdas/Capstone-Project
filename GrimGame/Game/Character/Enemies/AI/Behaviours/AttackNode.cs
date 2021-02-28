using GrimGame.Engine.AI;
using Microsoft.Xna.Framework;

namespace GrimGame.Game.Character.AI.Behaviours
{
    /// <summary>
    ///     Attack a given target if the enemy is within range of the target.
    /// </summary>
    public class AttackNode : BtNode
    {
        public const     float  MIN_DISTANCE_TO_TARGET = 32f;
        private readonly Enemy  _enemy;
        private readonly Player _target;
        private          float  _elapsedTime;

        public AttackNode(Player target, Enemy enemy)
        {
            _target = target;
            _enemy = enemy;
            _elapsedTime = 0;
        }

        public override Result Execute(GameTime gameTime)
        {
            // If enemy is still within range of the player
            if (_enemy.DistanceTo(_enemy.Position, _target.Position) <= MIN_DISTANCE_TO_TARGET)
            {
                _elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_elapsedTime >= _enemy.attackSpeed)
                {
                    // keep doing damage to player
                    _target.CurrentHp -= _enemy.attackDamage;
                    GrimDebugger.Log($"Done Damage: player's HP: {_target.CurrentHp}");
                    _elapsedTime -= _enemy.attackSpeed;
                    return Result.Running;
                }
            }

            // If the enemy is no longer in range of the player
            return Result.Failure;
        }
    }
}