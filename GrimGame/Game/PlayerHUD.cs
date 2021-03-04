﻿using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Game
{
    /// <summary>
    /// Displays basic information about the player's health and score.
    /// </summary>
    public class PlayerHud
    {
        private readonly SpriteFont _font = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private readonly Canvas     _canvas;
        private readonly Scene      _scene;
        private readonly Player     _player;

        public  bool    IsActive = true;
        private TextBox _hpTextBox;
        private TextBox _scoreTextBox;

        /// <summary>
        /// Create a new instance of the player's heads-up-display.
        /// </summary>
        /// <param name="scene">The scene to add the HUD to.</param>
        public PlayerHud(Scene scene)
        {
            _scene = scene;
            // create a new canvas
            _canvas = new Canvas();

            // Get the first instance of player
            _player = ObjectManager.Objects[ObjectManager.Objects.FindIndex(0, 1, o => o is Player)] as Player;

            // Create a new panel within the canvas
            var hpPanel = new Panel(Panel.Positions.BottomLeft, new Vector2(200, 50), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG") // TODO: Get a better texture
            };

            var scorePanel = new Panel(Panel.Positions.TopRight, new Vector2(200, 50), Color.White)
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
                    10),
                new Vector2(50, 20), Color.Blue);

            hpPanel.AddComponent(_hpTextBox);
            scorePanel.AddComponent(_scoreTextBox);
            _canvas.AddPanel(hpPanel);
            _canvas.AddPanel(scorePanel);
        }

        public void Update()
        {
            // Update player's health value
            _hpTextBox.SetText($"HP: {_player?.CurrentHp}", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));

            _scoreTextBox.SetText($"Score: {_player?.Score}", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));

            if (IsActive)
                _canvas.Update();
        }

        public void Draw()
        {
            if (IsActive)
                _canvas.Draw();
        }
    }
}