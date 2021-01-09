#region Imports

using System;
using GrimGame.Engine;
using GrimGame.Game.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace GrimGame.Game
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public MainGame()
        {
            var graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int) Globals.WindowSize.X,
                PreferredBackBufferHeight = (int) Globals.WindowSize.Y
            };

            graphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";

            Window.AllowUserResizing = false;
            IsMouseVisible = true;

            Globals.GameWindow = Window;

            Globals.Graphics = graphicsDeviceManager;
        }

        protected override void Initialize()
        {
            Globals.ContentManager = Content;
            Globals.GameTime = new GameTime();

            var (x, y) = Globals.VirtualSize;
            Globals.ViewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, (int) x, (int) y);

            Globals.Camera = new OrthographicCamera(Globals.ViewportAdapter);
            Globals.Camera.ZoomIn(1.2f);

            // Setup Level
            new Level1("Main Level", "StartLevel", this);

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