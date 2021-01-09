using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.GUI.Components
{
    public class TextBox : Component
    {
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

        public FontAlignment Alignment;
        public float         FontSize;
        public string        Text;
        public Color         TextColor;

        public TextBox(Vector2 position, Vector2 size, Color backgroundColor)
        {
            Position = position;
            Size = size;
            BackgroundColor = backgroundColor;
            Font = Globals.GuiFont;
            Bounds = new Rectangle(new Point((int) position.X, (int) position.Y),
                new Point((int) size.X, (int) size.Y));
        }

        private SpriteFont Font { get; set; }
        public float TextPadding { get; set; }

        public void SetText(string text, Color textColor, SpriteFont font,
            FontAlignment alignment = FontAlignment.CenterMiddle)
        {
            Text = text;
            TextColor = textColor;
            Font = font;
            Alignment = alignment;
        }

        public override void Draw()
        {
            var x = Bounds.X + Bounds.Width / 2 - Globals.GuiFont.MeasureString(Text).X;
            var y = Bounds.Y + Bounds.Height / 2 - Globals.GuiFont.MeasureString(Text).Y / 2;

            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.DrawString(Font, Text, new Vector2(x, y), TextColor);

            Globals.SpriteBatch.End();
        }

        public override void ReDraw()
        {
        }

        public override void Update()
        {
        }
    }
}