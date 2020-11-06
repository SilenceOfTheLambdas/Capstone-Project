using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Engine.GUI.Components
{
    public class Button : Component
    {
        #region Fields

        public readonly string Text;
        public Color TextColor;

        private Rectangle mouseBounds;
        
        public bool Hovering;
        private MouseState _currentMouseState;
        private MouseState _lastMouseState;
        private readonly Color _oldBackgroundColor;
        private readonly Color _oldTextColor;
        public SpriteFont Font;
        
        #endregion

        // _____ Properties _____ //
        public bool Clicked { get; set; }
        public event EventHandler Click;

        /// <summary>
        /// Create a new button component. A button is formed of a TextBox.
        /// </summary>
        public Button(string text, Vector2 position, Vector2 size, Color backgroundColor, Color textColor, SpriteFont font)
        {
            Position = position - new Vector2(size.X / 2, size.Y / 2);
            Text = text;
            Size = size;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            _oldTextColor = textColor;
            _oldBackgroundColor = BackgroundColor;
            Font = font;
            Bounds = new Rectangle(new Point((int) Position.X, (int) Position.Y), 
                new Point((int) Size.X, (int) Size.Y));
        }

        public override void Update()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            mouseBounds = new Rectangle(_currentMouseState.Position.X, _currentMouseState.Position.Y, 1, 1);

            Hovering = false;

            if (Bounds.Intersects(mouseBounds))
                Hovering = true;

            if (_currentMouseState.LeftButton == ButtonState.Released &&
                _lastMouseState.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
            }
            
            // Keep restoring old colors, unless the button is being hovered over
            BackgroundColor = _oldBackgroundColor;
            TextColor = _oldTextColor;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin();
            if (Hovering)
            {
                BackgroundColor = Color.Pink;
                TextColor = Color.Blue;
            }
            Globals.SpriteBatch.FillRectangle(Bounds, BackgroundColor);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Bounds.X + (Bounds.Width / 2)) - (Globals.GuiFont.MeasureString(Text).X / 2);
                var y = (Bounds.Y + (Bounds.Height / 2)) - (Globals.GuiFont.MeasureString(Text).Y / 2);
                
                Globals.SpriteBatch.DrawString(Font, Text, new Vector2(x, y), TextColor);
            }
            Globals.SpriteBatch.DrawRectangle(Bounds, Color.Pink);
            Globals.SpriteBatch.DrawRectangle(mouseBounds, Color.Orange);
            Globals.SpriteBatch.End();
        }

        public override void ReDraw()
        {

        }
    }
}