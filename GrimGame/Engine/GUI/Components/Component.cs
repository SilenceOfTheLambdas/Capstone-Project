using Microsoft.Xna.Framework;

namespace GrimGame.Engine.GUI.Components
{
    public abstract class Component
    {
        // _____ Transform _____ //
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Rectangle Bounds { get; set; }

        public Color BackgroundColor { get; set; }

        public abstract void Update();
        public abstract void Draw();

        public abstract void ReDraw();
    }
}