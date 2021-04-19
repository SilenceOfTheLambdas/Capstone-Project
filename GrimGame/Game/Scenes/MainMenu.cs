#nullable enable
using System;
using GrimGame.Engine;
using GrimGame.Engine.GUI;
using GrimGame.Engine.GUI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game.Scenes
{
    public class MainMenu : Scene
    {
        private const    int        ButtonSpace = 125;
        private readonly SpriteFont _buttonFont = Globals.ContentManager.Load<SpriteFont>("Fonts/buttonText");
        private          Canvas     _canvas;

        private readonly bool _isActive = true;

        private          Rectangle _mouseBounds;
        private          Button    _playButton;
        private          Button    _quitButton;
        private readonly Scene     _scene;

        public MainMenu(string sceneName, MainGame mainGame) : base(sceneName, mainGame)
        {
            _scene = this;
        }

        public override void Initialize()
        {
            // create a new canvas
            _canvas = new Canvas();

            Panel mainPanel = new Panel(Panel.Positions.CenterMiddle,
                new Vector2(Globals.Graphics.PreferredBackBufferWidth, Globals.Graphics.PreferredBackBufferHeight),
                Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Textures/GameMenuBackground")
            };

            // Create a new panel within the canvas
            Panel buttonPanel = new Panel(Panel.Positions.CenterMiddle, new Vector2(400, 200), Color.White)
            {
                Texture = Globals.ContentManager.Load<Texture2D>("Textures/GameMenuPanelBackground")
            };

            // Game title
            var gameTitle = new TextBox(
                buttonPanel.Position + new Vector2(
                    buttonPanel.Bounds.Width / 2 - Globals.GuiFont.MeasureString("GrimDoom").X / 2,
                    10), new Vector2(100, 50), Color.Blue);
            gameTitle.SetText("GrimDoom", Color.White,
                Globals.ContentManager.Load<SpriteFont>("Fonts/pauseMenuTitle"));

            // Play button
            _playButton = new Button("Play",
                buttonPanel.Position + new Vector2(buttonPanel.Size.X / 2, buttonPanel.Size.Y / 2),
                new Vector2(100, 40),
                Color.AntiqueWhite, Color.Black,
                _buttonFont) {ButtonHoverColor = Color.Gray, TextHoverColor = Color.White};

            // Quit button
            _quitButton = new Button("Quit",
                buttonPanel.Position + new Vector2(buttonPanel.Size.X / 2, _playButton.Size.Y + ButtonSpace),
                new Vector2(100, 40), Color.Red, Color.White, _buttonFont)
            {
                ButtonHoverColor = Color.DarkRed, TextHoverColor = Color.White
            };

            // Assign button event functions
            _playButton.Click += PlayButtonClick;
            _quitButton.Click += QuitButtonClick;

            buttonPanel.AddComponent(gameTitle);
            buttonPanel.AddComponent(_playButton);
            buttonPanel.AddComponent(_quitButton);
            _canvas.AddPanel(mainPanel);
            _canvas.AddPanel(buttonPanel);
            base.Initialize();
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
        ///     Called when the Resume button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButtonClick(object? sender, EventArgs e)
        {
            if (_playButton.Bounds.Intersects(_mouseBounds)) SceneManager.LoadScene("Main Level");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _mouseBounds = new Rectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1);

            if (_isActive)
                _canvas.Update();
        }

        public override void Draw()
        {
            base.Draw();
            if (_isActive)
                _canvas.Draw();
        }
    }
}