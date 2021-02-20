using GrimGame.Character.Enemies.AI;
using GrimGame.Engine;
using GrimGame.Engine.AI;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GrimGame.Game.Character.AI
{
    public class ChaseNode : BtNode
    {
        private readonly GameObject _target;
        private          Enemy      _enemy;
        private          PathFinder _pathFinder;
        
        public ChaseNode(GameObject target, Enemy enemy)
        {
            _target = target;
            _enemy = enemy;
            _pathFinder = new PathFinder(Globals.MapSystem)
            {
                Start = new Location {X = (int) _enemy.Position.X, Y = (int) _enemy.Position.Y},
                Target = new Location {X = (int) _target.Position.X, Y = (int) _target.Position.Y}
            };
        }

        public override Result Execute(GameTime gameTime)
        {
            var distance = Vector2.Distance(_target.Position, _enemy.Position);

            if (!(distance > 0.2f)) return Result.Success;

            _pathFinder.FindPath();

            _enemy.SetPosition(Vector2.Lerp(_enemy.Position, new Vector2(_pathFinder._current.X, _pathFinder._current.Y), gameTime.GetElapsedSeconds() / 20).X,
                Vector2.Lerp(_enemy.Position, new Vector2(_pathFinder._current.X, _pathFinder._current.Y), gameTime.GetElapsedSeconds() / 20).Y);
        
            /*while (current != null)
            {
                _enemy.SetPosition(Vector2.Lerp(_enemy.Position, new Vector2(3000, 2000), gameTime.GetElapsedSeconds() / 20).X,
                    Vector2.Lerp(_enemy.Position, new Vector2(3000, 2000), gameTime.GetElapsedSeconds() / 20).Y);
                current = current.Parent;
            }*/

            return Result.Running;

        }
    }
}