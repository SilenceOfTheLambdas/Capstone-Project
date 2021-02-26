using System.Collections.Generic;
using GrimGame.Character.Enemies.AI;
using GrimGame.Engine;
using GrimGame.Engine.AI;
using Microsoft.Xna.Framework;

namespace GrimGame.Game.Character.AI
{
    public class ChaseNode : BtNode
    {
        private readonly GameObject    _target;
        private readonly Enemy         _enemy;
        private readonly List<Vector2> _path;

        public ChaseNode(GameObject target, Enemy enemy)
        {
            _target = target;
            _enemy = enemy;
            var pathfinder = new Pathfinder(Globals.MapSystem.Map);

            _path = pathfinder.FindPath(new Point((int) _enemy.Position.X / 32, (int) _enemy.Position.Y / 32),
                new Point((int) _target.Position.X / 32, (int) _target.Position.Y / 32));
        }

        public override Result Execute(GameTime gameTime)
        {
            float elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            var distance = Vector2.Distance(_target.Position, _enemy.Position);

            if (!(distance > 0.2f)) return Result.Success;

            _path.ForEach(v => _enemy.MoveTowards(v, Globals.GameTime));
            return Result.Running;
        }
    }
}