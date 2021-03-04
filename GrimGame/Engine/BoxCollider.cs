#region Imports

using Microsoft.Xna.Framework;
using MonoGame.Extended;

#endregion

namespace GrimGame.Engine
{
    /// <summary>
    ///     A box collider is used to provide collision avoidance and detection for a <see cref="GameObject" />.
    /// </summary>
    public sealed class BoxCollider : GameObject
    {
        /// <summary>
        ///     Does this collider trigger an event?
        /// </summary>
        public bool IsTrigger = false;

        /// <summary>
        ///     Creates a new box collider
        /// </summary>
        /// <param name="origin">The origin point</param>
        /// <param name="size">The width and height of this box collider</param>
        public BoxCollider(Vector2 origin, Point2 size)
        {
            Origin = origin;
            Size = size;
        }

        /// <summary>
        ///     Updates the positions and size of the bounds.
        /// </summary>
        /// <param name="gameTime"><seealso cref="GameTime" />></param>
        public override void Update(GameTime gameTime)
        {
            Bounds = new Rectangle(new Point((int) Origin.X, (int) Origin.Y), new Point((int) Size.X, (int) Size.Y));
        }

        public void UpdatePosition(Point position)
        {
            Bounds.Location = position;
        }

        /// <summary>
        ///     UNUSED
        /// </summary>
        public override void Draw()
        {
        }
    }
}