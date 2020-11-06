using System.Collections.Generic;
using GrimGame.Engine.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.GUI
{
    public class Panel
    {
        // _____ Transform _____ //
        public Vector2 Position;
        public Vector2 Size;
        public Rectangle Bounds;

        // _____ Texture _____ //
        public Color BackgroundColor;

        public Texture2D Texture;

        /// <summary>
        /// A list of any sub-panels that are a child of this panel.
        /// </summary>
        public List<Panel> Panels;
        
        /// <summary>
        /// A list of any and all components part of this panel.
        /// </summary>
        public List<Component> Components;
        
        public Panel(Vector2 position, Vector2 size, Color backgroundColor)
        {
            this.Position = position;
            this.Size = size;
            this.BackgroundColor = backgroundColor;
            Panels = new List<Panel>();
            Components = new List<Component>();
            Bounds = new Rectangle(new Point((int) position.X, (int) position.Y), new Point((int) size.X, (int) size.Y));
        }

        public void AddComponent(Component component) => Components.Add(component);
        public void RemoveComponent(Component component) => Components.Remove(component);

        public void AddPanel(Panel panel) => Panels.Add(panel);
        public void RemovePanel(Panel panel) => Panels.Remove(panel);

        public void Draw()
        {
            Globals.SpriteBatch.Begin();
            if (Texture == null)
                Globals.SpriteBatch.Draw(new Texture2D(Globals.Graphics.GraphicsDevice, 
                    (int)Size.X, (int)Size.Y), new Rectangle((int) Position.X, (int) Position.Y, 
                    (int) Size.X, (int) Size.Y),
                new Rectangle(0, 0, (int)Size.X, (int)Size.Y), BackgroundColor);
            else
            {
                Globals.SpriteBatch.Draw(Texture, new Rectangle((int) Position.X, (int) Position.Y, 
                    (int) Size.X, (int) Size.Y),
                    new Rectangle(0, 0, Texture.Width, Texture.Height), BackgroundColor);
            }
            Globals.SpriteBatch.End();
            foreach (var component in Components)
            {
                component.Draw();
            }
            foreach (var panel in Panels)
            {
                panel.Draw();
            }
        }

        public void Update()
        {
            foreach (var panel in Panels)
            {
                panel.Update();
            }

            foreach (var component in Components)
            {
                component.Update();
            }
        }
    }
}