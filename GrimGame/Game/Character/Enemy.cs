using GrimGame.Engine;

namespace GrimGame.Game.Character
{
    /// <summary>
    ///     The base type for an enemy
    /// </summary>
    public abstract class Enemy : GameObject
    {
        /// <summary>
        ///     The current amount of health this Enemy has
        /// </summary>
        private int _currentHp;

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
                _currentHp = _maxHp;
            }
        }

        #endregion

        public void Kill()
        {
            Destroy(this);
        }
    }
}