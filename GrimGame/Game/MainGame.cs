﻿#region Imports
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
        // _____ Screen _____ //
        private const int Width = 1280; // Width of the viewport
        private const int Height = 720; // Height of the viewport

        public MainGame()
        {
            Globals.Graphics = new GraphicsDeviceManager(this);
            
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            Globals.Graphics.BeginDraw();
            Globals.Graphics.PreferredBackBufferWidth = Width;
            Globals.Graphics.PreferredBackBufferHeight = Height;
#if Linux
            Globals.Graphics.IsFullScreen = true;
#endif            
            Globals.Graphics.ApplyChanges();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            Globals.ContentManager = Content;
            Globals.GameTime = new GameTime();
            Globals.Camera = new OrthographicCamera(Globals.Graphics.GraphicsDevice);
            var viewportAdapter = new DefaultViewportAdapter(Globals.Graphics.GraphicsDevice);
            Globals.Camera = new OrthographicCamera(viewportAdapter);

            new Level1("Main Level", "TestMap", this);

            // Load scene
            SceneManager.LoadScene("Main Level");

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
            base.Update(gameTime);
            SceneManager.UpdateScenes(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SceneManager.DrawScenes(gameTime);
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