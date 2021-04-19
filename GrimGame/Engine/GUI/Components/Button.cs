using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Engine.GUI.Components
{
    /// <summary>
    ///     A button is a box containing text that can have an action assigned when a player clicks it.
    /// </summary>
    public sealed class Button : Component
    {
        /// <summary>
        ///     The current <see cref="MouseState" />
        /// </summary>
        private MouseState _currentMouseState;

        /// <summary>
        ///     Is the player hovering over this button?
        /// </summary>
        private bool _hovering;

        /// <summary>
        ///     The last known <see cref="MouseState" />.
        /// </summary>
        private MouseState _lastMouseState;

        /// <summary>
        ///     The bounds of the mouse cursor.
        /// </summary>
        private Rectangle _mouseBounds;

        /// <summary>
        ///     A button component.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="position">The position of the button</param>
        /// <param name="size">The width and height of the button</param>
        /// <param name="backgroundColor">The normal background colour</param>
        /// <param name="textColor">The normal text colour</param>
        /// <param name="font">The SpriteFont of the text in the button</param>
        public Button(string text, Vector2 position, Vector2 size, Color backgroundColor, Color textColor,
            SpriteFont font)
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

        /// <summary>
        ///     A button component.
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="position">The position of the button</param>
        /// <param name="size">The width and height of the button</param>
        /// <param name="backgroundColor">The normal background colour</param>
        /// <param name="textColor">The normal text colour</param>
        /// <param name="font">The SpriteFont of the text in the button</param>
        public Button(string text, Positions position, Vector2 size, Color backgroundColor, Color textColor,
            SpriteFont font)
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

        /// <summary>
        ///     Called when the player clicks on this button.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        ///     Update the mouse states, bounds, and active events when player is hovering over this button.
        /// </summary>
        public override void Update()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _mouseBounds = new Rectangle(_currentMouseState.Position.X, _currentMouseState.Position.Y, 1, 1);

            _hovering = Bounds.Intersects(_mouseBounds);

            if (_currentMouseState.LeftButton == ButtonState.Released &&
                _lastMouseState.LeftButton == ButtonState.Pressed)
                Click?.Invoke(this, new EventArgs());

            // Keep restoring old colors, unless the button is being hovered over
            BackgroundColor = _oldBackgroundColor;
            _textColor = _oldTextColor;
        }

        public void AddPadding(Vector2 padding)
        {
            Position += padding;
        }

        /// <summary>
        ///     Draws this button.
        /// </summary>
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
                var x = Bounds.X + Bounds.Width / 2 - Globals.GuiFont.MeasureString(_text).X / 2;
                var y = Bounds.Y + Bounds.Height / 2 - Globals.GuiFont.MeasureString(_text).Y / 2;

                Globals.SpriteBatch.DrawString(_font, _text, new Vector2(x, y), _textColor);
            }

            Globals.SpriteBatch.DrawRectangle(Bounds, Color.Pink);
            Globals.SpriteBatch.End();
        }

        #region Appearance

        /// <summary>
        ///     The background colour of this button when hovered over.
        /// </summary>
        public Color ButtonHoverColor { get; set; }

        /// <summary>
        ///     The text colour of this button when hovered over.
        /// </summary>
        public Color TextHoverColor { get; set; }

        /// <summary>
        ///     The <see cref="SpriteFont" /> to use for the text of this button.
        /// </summary>
        private readonly SpriteFont _font;

        /// <summary>
        ///     The text to display.
        /// </summary>
        private readonly string _text;

        /// <summary>
        ///     Current text <see cref="Color" />.
        /// </summary>
        private Color _textColor;

        /// <summary>
        ///     The original text <see cref="Color" />.
        /// </summary>
        private readonly Color _oldTextColor;

        /// <summary>
        ///     The original background <see cref="Color" />.
        /// </summary>
        private readonly Color _oldBackgroundColor;

        #endregion
    }
}