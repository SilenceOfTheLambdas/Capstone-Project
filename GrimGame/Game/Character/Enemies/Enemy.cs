using System.Collections.Generic;
using GrimGame.Character.Enemies.AI;
using GrimGame.Engine;
using Microsoft.Xna.Framework;

namespace GrimGame.Game.Character
{
    /// <summary>
    ///     The base type for an enemy
    /// </summary>
    public abstract class Enemy : GameObject
    {
        // Public variables
        public readonly Pathfinder PathFinder;
        private         int        _maxHp;

        // Private variables
        private Queue<Vector2> _waypoints;
        public  int            attackDamage = 5; // The *amount* of damage this enemy inflicts
        public  float          attackSpeed  = 1; // The timer for each enemy attack (in seconds)

        protected Enemy()
        {
            PathFinder = new Pathfinder(Globals.MapSystem.Map);
        }

        /// <summary>
        ///     Move this character to a given position.
        /// </summary>
        /// <param name="targetPosition">The position to move towards.</param>
        public void MoveTowards(Vector2 targetPosition)
        {
            // Generate the A* path
            var (x, y) = targetPosition;
            _waypoints = new Queue<Vector2>(PathFinder.FindPath(new Point((int) Position.X / 32, (int) Position.Y / 32),
                new Point((int) x / 32, (int) y / 32)));

            // Waypoint Logic
            if (_waypoints.Count > 0)
            {
                if (DistanceToDestination < Speed)
                {
                    Position = _waypoints.Peek();
                    _waypoints.Dequeue();
                }
                else
                {
                    Position += RadialMovement(_waypoints.Peek(), Position, Speed);
                    // TODO Rotation
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentHp <= 0)
                Kill();
        }

        private void Kill()
        {
            Destroy(this);
        }

        #region Properties

        public Queue<Vector2> Waypoints { get; }

        /// <summary>
        ///     The maximum amount of health points this enemy can have
        /// </summary>
        public int MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                CurrentHp = _maxHp;
            }
        }

        /// <summary>
        ///     The current amount of health this Enemy has
        /// </summary>
        public int CurrentHp { get; set; }

        public float DistanceToDestination => _waypoints.Count > 0 ? Vector2.Distance(Position, _waypoints.Peek()) : 0f;

        #endregion
    }
}