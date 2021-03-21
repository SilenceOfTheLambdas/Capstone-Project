using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Game
{
    /// <summary>
    ///     Displays basic information about the player's health and score.
    /// </summary>
    public class PlayerHud
    {
        public const     bool    IsActive = true;
        private readonly Canvas  _canvas;
        private          TextBox _hpTextBox;
        private          Player  _player;
        private          TextBox _scoreTextBox;
        private          TextBox _coinTextBox;

        /// <summary>
        ///     Create a new instance of the player's heads-up-display.
        /// </summary>
        public PlayerHud()
        {
            // create a new canvas
            _canvas = new Canvas();
        }

        public void Init()
        {
            // Get the first instance of player
            _player = SceneManager.GetActiveScene.ObjectManager.Objects[
                SceneManager.GetActiveScene.ObjectManager.Objects.FindIndex(0,
                    1,
                    o => o is Player)] as Player;

            // Create a new panel within the canvas
            var hpPanel = new Panel(Panel.Positions.BottomLeft, new Vector2(200, 50), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG") // TODO: Get a better texture
            };

            var scorePanel = new Panel(Panel.Positions.TopRight, new Vector2(200, 80), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };

            // Health text
            _hpTextBox = new TextBox(
                hpPanel.Position + new Vector2(
                    hpPanel.Bounds.Width / 2 - Globals.GuiFont.MeasureString(
                        // Get the first object that is the player
                        $"HP: {_player?.CurrentHp}").X / 2,
                    10),
                new Vector2(50, 20), Color.Blue);

            // Score text
            _scoreTextBox = new TextBox(
                scorePanel.Position + new Vector2(
                    scorePanel.Bounds.Width / 2 - Globals.GuiFont.MeasureString(
                        // Get the first object that is the player
                        $"Score: {_player?.Score}").X / 2,
                    40),
                new Vector2(50, 20), Color.Blue);

            _coinTextBox = new TextBox(
                scorePanel.Position + new Vector2(
                    scorePanel.Bounds.Width / 2 - Globals.GuiFont.MeasureString(
                        // Get the first object that is the player
                        $"Coins: {_player?.Coins}").X / 2,
                    14),
                new Vector2(50, 20), Color.Blue);

            hpPanel.AddComponent(_hpTextBox);
            scorePanel.AddComponent(_scoreTextBox);
            scorePanel.AddComponent(_coinTextBox);
            _canvas.AddPanel(hpPanel);
            _canvas.AddPanel(scorePanel);
        }

        public void Update()
        {
            // Update player's health value
            _hpTextBox?.SetText($"HP: {_player?.CurrentHp}", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));

            _scoreTextBox?.SetText($"Score: {_player?.Score}", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));

            _coinTextBox?.SetText($"Coins: {_player?.Coins}", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));


            _canvas.Update();
        }

        public void Draw()
        {
            _canvas.Draw();
        }
    }
}