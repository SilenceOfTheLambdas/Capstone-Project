using System;
using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    public class EndGameMenu
    {
        private readonly SpriteFont _buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private readonly Canvas     _canvas;
        private readonly Button     _mainMenuButton;
        private readonly Button     _quitButton;
        private readonly Scene      _scene;

        private Rectangle _mouseBounds;

        public  bool    IsActive;
        private TextBox _scorePanelTextBox;
        private Player? _player;

        /// <summary>
        ///     Creates a pause menu, displaying options: Resume and Quit.
        /// </summary>
        /// <param name="scene">The scene in which to add the pause menu</param>
        public EndGameMenu(Scene scene)
        {
            _scene = scene;

            // create a new canvas
            _canvas = new Canvas();

            // Create a new panel within the canvas
            Panel panel = new Panel(Panel.Positions.CenterMiddle, new Vector2(400, 400), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };

            _scorePanelTextBox = new TextBox(panel.Position, new Vector2(300, 150), Color.White);
            // Get the player's score
            _player = SceneManager.GetActiveScene.ObjectManager.Objects.Find(o => o is Player) as Player;
            _scorePanelTextBox.SetText($"Total Score: {_player?.Score}", Color.Red, _buttonFont);

            // Return to Main Menu button
            _mainMenuButton = new Button("Return to MainMenu", panel.Position + new Vector2(panel.Size.X / 2, 160),
                new Vector2(200, 40), Color.Green, Color.White, _buttonFont)
            {
                ButtonHoverColor = Color.DarkGreen, TextHoverColor = Color.White
            };

            // Quit button
            _quitButton = new Button("Quit", panel.Position + new Vector2(panel.Size.X / 2, panel.Size.Y - 50),
                new Vector2(180, 40), Color.Red, Color.White, _buttonFont)
            {
                ButtonHoverColor = Color.DarkRed, TextHoverColor = Color.White
            };

            // Assign button event functions
            _mainMenuButton.Click += BackToMainMenuClick;
            _quitButton.Click += QuitButtonClick;
            panel.AddComponent(_scorePanelTextBox);
            panel.AddComponent(_mainMenuButton);
            panel.AddComponent(_quitButton);
            _canvas.AddPanel(panel);
        }

        /// <summary>
        ///     Called when the quit button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitButtonClick(object? sender, EventArgs e)
        {
            if (_quitButton.Bounds.Intersects(_mouseBounds))
                _scene.MainGame.Exit();
        }

        /// <summary>
        ///     Returns the player back to the main menu scene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMainMenuClick(object? sender, EventArgs e)
        {
            if (_mainMenuButton.Bounds.Intersects(_mouseBounds))
                SceneManager.LoadScene("Main Menu");
        }

        public void Update()
        {
            _mouseBounds = new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1);
            _scorePanelTextBox.SetText($"Total Score: {_player?.Score}", Color.Red, _buttonFont);
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