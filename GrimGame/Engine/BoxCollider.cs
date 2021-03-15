#region Imports

using Microsoft.Xna.Framework;
using MonoGame.Extended;

#endregion

namespace GrimGame.Engine
{
    /// <summary>
    ///     A box collider is used to provide collision avoidance and detection for a <see cref="GameObject" />.
    /// </summary>
    public sealed class BoxCollider
    {
        /// <summary>
        ///     Does this collider trigger an event?
        /// </summary>
        public bool IsTrigger = false;

        public Vector2   Origin;
        public Point2    Size;
        public Rectangle Bounds;

        /// <summary>
        ///     Creates a new box collider
        /// </summary>
        /// <param name="origin">The origin point</param>
        /// <param name="size">The width and height of this box collider</param>
        public BoxCollider(Vector2 origin, Point2 size)
        {
            Origin = origin;
            Size = size;
            Bounds = new Rectangle(new Point((int) Origin.X, (int) Origin.Y), new Point((int) Size.X, (int) Size.Y));
        }

        public void UpdatePosition(Point position)
        {
            Bounds.Location = position;
            var _ = new Vector2(position.X, position.Y);
        }
    }
}