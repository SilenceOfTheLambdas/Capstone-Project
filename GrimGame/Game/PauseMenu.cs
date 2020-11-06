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
    public class PauseMenu
    {
        private readonly Canvas _canvas;
        private readonly Button _resumeButton;
        private readonly Button _quitButton;
        private readonly SpriteFont _buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private readonly MainGame _game;

        public bool IsActive;
        private const int ButtonSpace = 125;

        private Rectangle _mouseBounds;
        
        /// <summary>
        /// Creates a pause menu, displaying options: Resume and Quit.
        /// </summary>
        /// <param name="game">A reference to the main game.</param>
        public PauseMenu(MainGame game)
        {
            _canvas = new Canvas();
            _game = game;
            Panel panel = new Panel(new Vector2(1920 / 2 - 200,
                1080 / 2 - 100), new Vector2(400, 200), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Debugging/DB_BG")
            };
            TextBox pauseMenuTitle = new TextBox(
                panel.Position + new Vector2(panel.Bounds.Width / 2 - (Globals.GuiFont.MeasureString("Paused").X / 2),
                    10), 
                new Vector2(100, 50), Color.Blue);
            pauseMenuTitle.SetText("Paused", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));
            
            _resumeButton = new Button("Resume", panel.Position + new Vector2(panel.Size.X / 2, panel.Size.Y / 2), 
                new Vector2(100, 40),
                Color.AntiqueWhite, Color.Black,
                _buttonFont);

            _quitButton = new Button("Quit",
                panel.Position + new Vector2(panel.Size.X / 2, _resumeButton.Size.Y + ButtonSpace ),
                new Vector2(100, 40), Color.Red, Color.White, _buttonFont);
            
            _resumeButton.Click += ResumeButtonClick;
            _quitButton.Click += QuitButtonClick;
            
            panel.AddComponent(pauseMenuTitle);
            panel.AddComponent(_resumeButton);
            panel.AddComponent(_quitButton);
            _canvas.AddPanel(panel);
        }

        private void QuitButtonClick(object? sender, EventArgs e)
        {
            if (_quitButton.Bounds.Intersects(_mouseBounds))
                _game.Exit();
        }

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