using System;
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
        /// <summary>
        ///     The amount of damage this enemy inflicts.
        /// </summary>
        public const int AttackDamage = 5;

        /// <summary>
        ///     The timer for each attack (in seconds)
        /// </summary>
        public const float AttackSpeed = 1;

        // Private variables
        /// <summary>
        ///     The 'path' this enemy has to follow to get to a position. Makes use of the <see cref="Pathfinder" /> system.
        /// </summary>
        private static Queue<Vector2> _waypoints;

        /// <summary>
        ///     A reference to this enemy's <see cref="Pathfinder" />.
        /// </summary>
        private readonly Pathfinder _pathFinder;

        private int _maxHp;

        // Public variables
        /// <summary>
        ///     The animation manager for this enemy.
        /// </summary>
        protected AnimationManager AnimationManager;

        protected Enemy()
        {
            _pathFinder = new Pathfinder(Globals.MapSystem.Map);
            ObjectManager.Objects.Add(this);
        }

        /// <summary>
        ///     Move this character to a given position.
        /// </summary>
        /// <param name="targetPosition">The position to move towards.</param>
        public void MoveTowards(Vector2 targetPosition)
        {
            // Generate the A* path
            var (x, y) = targetPosition;
            _waypoints = new Queue<Vector2>(_pathFinder.FindPath(
                new Point((int) Position.X / 32, (int) Position.Y / 32),
                new Point((int) x / 32, (int) y / 32)));

            // Waypoint Logic
            if (_waypoints.Count <= 0) return;

            if (DistanceToDestination < Speed)
            {
                Position = _waypoints.Peek();
                _waypoints.Dequeue();
            }
            else
            {
                Position += RadialMovement(_waypoints.Peek(), Position, Speed);

                // Update the enemies' rotation and direction
                var direction = _waypoints.Peek() - Position;
                direction.Normalize();
                var rotationInRadians = (int) ((int) Math.Atan2(direction.Y,
                    direction.X) + MathHelper.PiOver2);
                switch (rotationInRadians)
                {
                    case 0:
                        AnimationManager.Play(Sprite.Animations["walk_up"]);
                        break;
                    case 1:
                        AnimationManager.Play(Sprite.Animations["walk_right"]);
                        break;
                    case 2:
                        AnimationManager.Play(Sprite.Animations["walk_down"]);
                        break;
                    case 3:
                        AnimationManager.Play(Sprite.Animations["walk_left"]);
                        break;
                    default:
                        AnimationManager.Stop();
                        break;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentHp <= 0)
                Kill();
        }

        /// <summary>
        ///     Killing this enemy will destroy the <see cref="GameObject" />.
        /// </summary>
        private void Kill()
        {
            Destroy(this);
        }

        #region Properties

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

        /// <summary>
        ///     The distance from the current position to the next point in <see cref="_waypoints" />.
        /// </summary>
        public float DistanceToDestination => _waypoints.Count > 0 ? Vector2.Distance(Position, _waypoints.Peek()) : 0f;

        #endregion
    }
}