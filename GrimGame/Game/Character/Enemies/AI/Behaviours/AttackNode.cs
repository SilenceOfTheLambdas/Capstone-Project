using GrimGame.Engine;
using GrimGame.Engine.AI;
using Microsoft.Xna.Framework;

namespace GrimGame.Game.Character.AI.Behaviours
{
    /// <summary>
    ///     Attack a given target if the enemy is within range of the target.
    /// </summary>
    public class AttackNode : BtNode
    {
        private const    float  MinDistanceToTarget = 32f;
        private readonly Enemy  _enemy;
        private readonly Player _target;
        private          float  _elapsedTime;

        /// <summary>
        ///     Creates a new attacking behaviour.
        /// </summary>
        /// <param name="target">The target to attack</param>
        /// <param name="enemy">The enemy performing the attack</param>
        public AttackNode(Player target, Enemy enemy)
        {
            _target = target;
            _enemy = enemy;
            _elapsedTime = 0;
        }

        public override Result Execute(GameTime gameTime)
        {
            // If enemy is still within range of the player
            if (GameObject.GetDistance(_enemy.Position, _target.Position) <= MinDistanceToTarget)
            {
                _elapsedTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_elapsedTime >= Enemy.AttackSpeed)
                {
                    // keep doing damage to player
                    _target.CurrentHp -= Enemy.AttackDamage;
                    GrimDebugger.Log($"Done Damage: player's HP: {_target.CurrentHp}");
                    _elapsedTime -= Enemy.AttackSpeed;
                    return Result.Running;
                }
            }

            // If the enemy is no longer in range of the player
            return Result.Failure;
        }
    }
}