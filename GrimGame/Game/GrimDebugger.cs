using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GrimGame.Game
{
    public class GrimDebugger
    {
        // _____ References _____ //
        private readonly Player _player;
        private readonly MapSystem _mapSystem;
        private readonly SpriteFont _debugFont;

        // _____ Output _____ //
        private static string _outputText;
        
        // _____ Properties _____ //
        private const int TextPadding = 4;
        private readonly Color _gridColour = Color.MonoGameOrange;

        public GrimDebugger(Player player, MapSystem mapSystem,SpriteFont debugFont)
        {
            this._player = player;
            this._mapSystem = mapSystem;
            this._debugFont = debugFont;
            _gridColour.A = byte.Parse("1");
        }

        /// <summary>
        /// Log some text into the debug menu.
        /// </summary>
        /// <param name="text">The String to output</param>
        public static void Log(string text)
        {
            _outputText += "\n";
            _outputText += text;
        }

        public void Draw()
        {
            _outputText = ($"Player position: {_player.Position}" + "\n" +
                           $"Player Tile Position: {_player.TilePosition}" + "\n" +
                           $"Player Index: {_mapSystem.CurrentIndex}");
            
            Globals.SpriteBatch.Begin();
            var panelPosition = new Vector2(0, 0);
            var textMiddlePoint = _debugFont.MeasureString(_outputText);
            var textPosition = new Vector2(textMiddlePoint.X + TextPadding, panelPosition.Y + (textMiddlePoint.Y + TextPadding));
                
            Globals.SpriteBatch.Draw(Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG"), panelPosition, Color.White);
            Globals.SpriteBatch.DrawString(_debugFont, _outputText, textPosition, Color.White,
                0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            Globals.SpriteBatch.End();
                
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
            Globals.SpriteBatch.DrawPoint(_mapSystem.Map.ObjectLayers[1].Objects[0].Position,Color.Green);
            Globals.SpriteBatch.End();

            DrawPlayerBounds();
            DrawCollisionObjects();
        }

        /// <summary>
        /// Draws a grid over the map.
        /// </summary>
        public void DrawGrid()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
            for (var x1 = 0; x1 <= (100 * 32); x1 += 32)
            {
                for (var y1 = 0; y1 <= 100 * 32; y1 += 32)
                {
                    Globals.SpriteBatch.DrawRectangle(new Vector2(x1, y1), new Size2(32, 32), _gridColour);
                }
            }
            Globals.SpriteBatch.End();
        }

        private void DrawCollisionObjects()
        {
            foreach (var collisionObject in _mapSystem.CollisionObjects)
            {
                Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
                Globals.SpriteBatch.DrawRectangle(collisionObject, Color.Gray);
                Globals.SpriteBatch.End();
            }
        }

        private void DrawPlayerBounds()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
            Globals.SpriteBatch.DrawRectangle(_player.BoxCollider.Bounds, Color.Purple);
            Globals.SpriteBatch.End();
        }

        public static void DrawRectangle(Rectangle r, Color color)
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
            Globals.SpriteBatch.DrawRectangle(r, color);
            Globals.SpriteBatch.End();
        }
    }
}