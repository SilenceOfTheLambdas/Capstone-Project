using Microsoft.Xna.Framework;

namespace GrimGame.Engine.GUI.Components
{
    /// <summary>
    ///     A <see cref="Component" /> is an item that can be displayed on a panel.
    /// </summary>
    public abstract class Component
    {
        // _____ Transform _____ //

        /// <summary>
        ///     The position of this component.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        ///     The size if this component.
        /// </summary>
        public Vector2 Size { get; protected set; }

        /// <summary>
        ///     The <see cref="Rectangle" /> pertaining to the bounds of this component.
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        /// <summary>
        ///     Background colour mask.
        /// </summary>
        protected Color BackgroundColor { get; set; }

        /// <summary>
        ///     Updates component information, this is called every frame.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Draws the component.
        /// </summary>
        public abstract void Draw();
    }
}