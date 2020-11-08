using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Engine.GUI.Components
{
    public class Button : Component
    {
        #region Appearance

        public Color ButtonHoverColor { get; set; }
        public Color TextHoverColor { get; set; }
        private readonly SpriteFont _font;
        private readonly string _text;
        private Color _textColor;
        private readonly Color _oldTextColor;
        private readonly Color _oldBackgroundColor;
        
        #endregion
        
        private bool _hovering;
        private MouseState _currentMouseState;
        private MouseState _lastMouseState;
        private Rectangle _mouseBounds;
        
        public event EventHandler Click;

        /// <summary>
        /// A button component.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="position">The position of the button</param>
        /// <param name="size">The width and height of the button</param>
        /// <param name="backgroundColor">The normal background colour</param>
        /// <param name="textColor">The normal text colour</param>
        /// <param name="font">The SpriteFont of the text in the button</param>
        public Button(string text, Vector2 position, Vector2 size, Color backgroundColor, Color textColor, SpriteFont font)
        {
            Position = position - new Vector2(size.X / 2, size.Y / 2);
            _text = text;
            Size = size;
            BackgroundColor = backgroundColor;
            _textColor = textColor;
            _oldTextColor = textColor;
            _oldBackgroundColor = BackgroundColor;
            _font = font;
            Bounds = new Rectangle(new Point((int) Position.X, (int) Position.Y), 
                new Point((int) Size.X, (int) Size.Y));
        }

        public override void Update()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _mouseBounds = new Rectangle(_currentMouseState.Position.X, _currentMouseState.Position.Y, 1, 1);

            _hovering = Bounds.Intersects(_mouseBounds);

            if (_currentMouseState.LeftButton == ButtonState.Released &&
                _lastMouseState.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
            }
            
            // Keep restoring old colors, unless the button is being hovered over
            BackgroundColor = _oldBackgroundColor;
            _textColor = _oldTextColor;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin();
            if (_hovering)
            {
                BackgroundColor = ButtonHoverColor;
                _textColor = TextHoverColor;
            }
            Globals.SpriteBatch.FillRectangle(Bounds, BackgroundColor);

            if (!string.IsNullOrEmpty(_text))
            {
                var x = (Bounds.X + (Bounds.Width / 2)) - (Globals.GuiFont.MeasureString(_text).X / 2);
                var y = (Bounds.Y + (Bounds.Height / 2)) - (Globals.GuiFont.MeasureString(_text).Y / 2);
                
                Globals.SpriteBatch.DrawString(_font, _text, new Vector2(x, y), _textColor);
            }
            Globals.SpriteBatch.DrawRectangle(Bounds, Color.Pink);
            Globals.SpriteBatch.DrawRectangle(_mouseBounds, Color.Orange);
            Globals.SpriteBatch.End();
        }

        public override void ReDraw()
        {

        }
    }
}