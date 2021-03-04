using System;
using System.Collections.Generic;
using GrimGame.Engine.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.GUI
{
    /// <summary>
    ///     A panel is held within a canvas, and holds various <see cref="Component" />s.
    /// </summary>
    public class Panel
    {
        /// <summary>
        ///     A list of fixed positions this panel can be positioned.
        /// </summary>
        public enum Positions
        {
            CenterLeft,
            CenterMiddle,
            CenterRight,
            TopLeft,
            TopMiddle,
            TopRight,
            BottomLeft,
            BottomMiddle,
            BottomRight
        }

        // _____ Texture _____ //

        /// <summary>
        ///     The background masking colour of this panel.
        /// </summary>
        private readonly Color _backgroundColor;

        /// <summary>
        ///     A list of any and all components part of this panel.
        /// </summary>
        private readonly List<Component> _components;

        /// <summary>
        ///     A list of any sub-panels that are a child of this panel.
        /// </summary>
        private readonly List<Panel> _panels;

        /// <summary>
        ///     The bounds of this panel.
        /// </summary>
        public Rectangle Bounds;

        // _____ Transform _____ //

        /// <summary>
        ///     Position of the panel on the screen.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        ///     Size of this panel.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        ///     The texture this panel uses.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        ///     Creates a new panel.
        /// </summary>
        /// <param name="position">Make a panel at one of the specified positions</param>
        /// <param name="size">The size of the panel</param>
        /// <param name="backgroundColor">The background colour mask</param>
        public Panel(Positions position, Vector2 size, Color backgroundColor)
        {
            Position = position switch
            {
                Positions.TopMiddle => new Vector2(Globals.Graphics.PreferredBackBufferWidth / 2 - size.X / 2, 0),
                Positions.CenterMiddle => new Vector2(Globals.Graphics.PreferredBackBufferWidth / 2 - size.X / 2,
                    Globals.Graphics.PreferredBackBufferHeight / 2 - size.Y / 2),
                Positions.CenterLeft => new Vector2(0, Globals.Graphics.PreferredBackBufferHeight / 2 - size.Y / 2),
                Positions.CenterRight => new Vector2(Globals.Graphics.PreferredBackBufferWidth - size.X,
                    Globals.Graphics.PreferredBackBufferHeight / 2 - size.Y / 2),
                Positions.TopLeft => new Vector2(0, 0),
                Positions.TopRight => new Vector2(Globals.Graphics.PreferredBackBufferWidth - size.X, 0),
                Positions.BottomLeft => new Vector2(0, Globals.Graphics.PreferredBackBufferHeight - size.Y),
                Positions.BottomMiddle => new Vector2(Globals.Graphics.PreferredBackBufferWidth / 2 - size.X / 2,
                    Globals.Graphics.PreferredBackBufferHeight - size.Y),
                Positions.BottomRight => new Vector2(Globals.Graphics.PreferredBackBufferWidth - size.X,
                    Globals.Graphics.PreferredBackBufferHeight - size.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
            };
            Size = size;
            _backgroundColor = backgroundColor;
            _panels = new List<Panel>();
            _components = new List<Component>();
            Bounds = new Rectangle(new Point((int) Position.X, (int) Position.Y),
                new Point((int) size.X, (int) size.Y));
        }

        /// <summary>
        ///     Add a component to this panel.
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(Component component)
        {
            _components.Add(component);
        }

        /// <summary>
        ///     Removes a component from the panel.
        /// </summary>
        /// <param name="component">The component to remove</param>
        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        /// <summary>
        ///     You can add other panels within this panel.
        /// </summary>
        /// <param name="panel">The panel to add</param>
        public void AddPanel(Panel panel)
        {
            _panels.Add(panel);
        }

        /// <summary>
        ///     Remove a panel from within this panel.
        /// </summary>
        /// <param name="panel">The panel to remove</param>
        public void RemovePanel(Panel panel)
        {
            _panels.Remove(panel);
        }

        /// <summary>
        ///     Draws every <see cref="Component" />, Panel, and panels within this panel.
        /// </summary>
        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            if (Texture == null)
                Globals.SpriteBatch.Draw(new Texture2D(Globals.Graphics.GraphicsDevice,
                        (int) Size.X, (int) Size.Y), new Rectangle((int) Position.X, (int) Position.Y,
                        (int) Size.X, (int) Size.Y),
                    new Rectangle(0, 0, (int) Size.X, (int) Size.Y), _backgroundColor);
            else
                Globals.SpriteBatch.Draw(Texture, Bounds,
                    new Rectangle(0, 0, Texture.Width, Texture.Height), _backgroundColor);
            Globals.SpriteBatch.End();
            foreach (var component in _components) component.Draw();
            foreach (var panel in _panels) panel.Draw();
        }

        /// <summary>
        ///     Updates every <see cref="Component" /> and panels within this panel.
        /// </summary>
        public void Update()
        {
            foreach (var panel in _panels) panel.Update();

            foreach (var component in _components) component.Update();
        }
    }
}