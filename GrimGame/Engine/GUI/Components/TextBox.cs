using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.GUI.Components
{
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

    /// <summary>
    ///     A text box displays text.
    /// </summary>
    public sealed class TextBox : Component
    {
        /// <summary>
        ///     A set of alignments (positions) the text within this box can be located.
        /// </summary>
        public enum FontAlignment
        {
            LeftUpper,
            LeftMiddle,
            LeftBottom,
            CenterUpper,
            CenterMiddle,
            CenterBottom,
            RightUpper,
            RightMiddle,
            RightBottom
        }

        /// <summary>
        ///     The text to display.
        /// </summary>
        private string _text;

        /// <summary>
        ///     The <see cref="Color" /> of the text.
        /// </summary>
        private Color _textColor;

        /// <summary>
        ///     The size of the font.
        /// </summary>
        public float FontSize;

        /// <summary>
        ///     Creates a new text box.
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="size">The total size</param>
        /// <param name="backgroundColor">The background colour mask</param>
        public TextBox(Vector2 position, Vector2 size, Color backgroundColor)
        {
            Position = position;
            Size = size;
            BackgroundColor = backgroundColor;
            Font = Globals.GuiFont;
            Bounds = new Rectangle(new Point((int) position.X, (int) position.Y),
                new Point((int) size.X, (int) size.Y));
        }

        public TextBox(Positions position, Vector2 size, Color backgroundColor)
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
            BackgroundColor = backgroundColor;
            Font = Globals.GuiFont;
            Bounds = new Rectangle(new Point((int) Position.X, (int) Position.Y),
                new Point((int) size.X, (int) size.Y));
        }

        /// <summary>
        ///     The font of the text.
        /// </summary>
        private SpriteFont Font { get; set; }

        /// <summary>
        ///     Set the text of this text box.
        /// </summary>
        /// <param name="text">The <see cref="string" /> to display</param>
        /// <param name="textColor">The <see cref="Color" /> of the text</param>
        /// <param name="font">The <see cref="SpriteFont" /> of the text</param>
        /// <param name="alignment">Alignment</param>
        public void SetText(string text, Color textColor, SpriteFont font,
            FontAlignment alignment = FontAlignment.CenterMiddle)
        {
            _text = text;
            _textColor = textColor;
            Font = font;
        }

        public void AddText(string text)
        {
            _text += text;
        }

        /// <summary>
        ///     Draws the text box and text within it.
        /// </summary>
        public override void Draw()
        {
            var x = Bounds.X + Bounds.Width / 2 - Globals.GuiFont.MeasureString(_text).X;
            var y = Bounds.Y + Bounds.Height / 2 - Globals.GuiFont.MeasureString(_text).Y / 2;

            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.DrawString(Font, _text, new Vector2(x, y), _textColor);

            Globals.SpriteBatch.End();
        }

        /// <summary>
        ///     UNUSED
        /// </summary>
        public override void Update()
        {
        }
    }
}