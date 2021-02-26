using GrimGame.Engine;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GrimGame.Game.Character
{
    /// <summary>
    ///     The base type for an enemy
    /// </summary>
    public abstract class Enemy : GameObject
    {
        private int _maxHp;

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

        #endregion

        public void MoveTowards(Vector2 targetPosition, GameTime gameTime)
        {
            Position = Vector2.Lerp(Position, targetPosition, Speed * gameTime.GetElapsedSeconds());
        }

        public void Kill()
        {
            Destroy(this);
        }
    }
}