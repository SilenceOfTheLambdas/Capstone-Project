#nullable enable
using System;
using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    /// <summary>
    ///     A pause menu that gives the player options to Resume and Quit.
    /// </summary>
    public class PauseMenu
    {
        private readonly SpriteFont _buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private readonly Canvas     _canvas;
        private readonly Button     _mainMenuButton;
        private readonly Button     _resumeButton;
        private readonly Button     _quitButton;
        private readonly Scene      _scene;

        private Rectangle _mouseBounds;

        public bool IsActive;

        /// <summary>
        ///     Creates a pause menu, displaying options: Resume and Quit.
        /// </summary>
        /// <param name="scene">The scene in which to add the pause menu</param>
        public PauseMenu(Scene scene)
        {
            _scene = scene;

            // create a new canvas
            _canvas = new Canvas();

            // Create a new panel within the canvas
            Panel panel = new Panel(Panel.Positions.CenterMiddle, new Vector2(400, 200), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };

            // Resume button
            _resumeButton = new Button("Resume", panel.Position + new Vector2(panel.Size.X / 2, panel.Size.Y / 2),
                new Vector2(100, 40),
                Color.AntiqueWhite, Color.Black,
                _buttonFont) {ButtonHoverColor = Color.Gray, TextHoverColor = Color.White};

            // Return to Main Menu button
            _mainMenuButton = new Button("MainMenu",
                panel.Position + new Vector2(panel.Size.X / 2, _resumeButton.Size.Y),
                new Vector2(100, 40), Color.Green, Color.White, _buttonFont)
            {
                ButtonHoverColor = Color.DarkGreen, TextHoverColor = Color.White
            };

            // Quit button
            _quitButton = new Button("Quit",
                panel.Position + new Vector2(panel.Size.X / 2,
                    160),
                new Vector2(100, 40), Color.Red, Color.White, _buttonFont)
            {
                ButtonHoverColor = Color.DarkRed, TextHoverColor = Color.White
            };

            // Assign button event functions
            _resumeButton.Click += ResumeButtonClick;
            _mainMenuButton.Click += BackToMainMenuClick;
            _quitButton.Click += QuitButtonClick;

            panel.AddComponent(_resumeButton);
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
        /// Returns the player back to the main menu scene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMainMenuClick(object? sender, EventArgs e)
        {
            if (_mainMenuButton.Bounds.Intersects(_mouseBounds))
                SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        ///     Called when the Resume button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResumeButtonClick(object? sender, EventArgs e)
        {
            if (_resumeButton.Bounds.Intersects(_mouseBounds))
                IsActive = false;
        }

        public void Update()
        {
            _mouseBounds = new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1);

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