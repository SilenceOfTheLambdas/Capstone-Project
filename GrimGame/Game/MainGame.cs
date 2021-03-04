#region Imports

using System;
using GrimGame.Engine;
using GrimGame.Game.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace GrimGame.Game
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public MainGame()
        {
            var graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.IsFullScreen = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Globals.GameWindow = Window;
            Globals.Graphics = graphicsDeviceManager;
        }

        protected override void Initialize()
        {
            // Init Globals
            Globals.ContentManager = Content;
            Globals.GameTime = new GameTime();

            var (x, y) = Globals.VirtualSize;
            Globals.ViewportAdapter =
                new BoxingViewportAdapter(Window, Globals.Graphics.GraphicsDevice, (int) x, (int) y);
            Globals.Camera = new OrthographicCamera(Globals.ViewportAdapter);
            Globals.Camera.ZoomIn(1.6f);
            Globals.Graphics.ApplyChangesSafely();

            Globals.Graphics.PreferredBackBufferWidth = 1280;
            Globals.Graphics.PreferredBackBufferHeight = 720;

            Globals.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth =
                Globals.Graphics.PreferredBackBufferWidth;
            Globals.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight =
                Globals.Graphics.PreferredBackBufferHeight;
            Globals.Graphics.GraphicsDevice.Viewport = new Viewport(0, 0, Globals.Graphics.PreferredBackBufferWidth,
                Globals.Graphics.PreferredBackBufferHeight);
            Globals.Graphics.ApplyChangesSafely();

            // Setup Level
            var level1 = new Level1("Main Level", "StartLevel", this);

            // Load scene
            SceneManager.LoadScene("Main Level");

            base.Initialize();

            SceneManager.InitScenes();
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
            Globals.DebugFont = Content.Load<SpriteFont>("Fonts/debugFont");
            Globals.GuiFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            SceneManager.UpdateScenes(gameTime);
            // Updating Globals
            Globals.GameTime = gameTime;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.DrawScenes(gameTime);
            base.Draw(gameTime);
        }
    }

    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new MainGame();
            game.Run();
        }
    }
}