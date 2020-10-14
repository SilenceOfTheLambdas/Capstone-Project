#region Imports
using System;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
#endregion

namespace GrimGame.Game
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private Player _player;

        // _____ Map System _____ //
        private MapSystem _mapSystem;

        // _____ Debug _____ //
        private bool _showDebug;
        private SpriteFont _debugFont;
        
        public MainGame()
        {
            var graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the window size
            graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            
            // _____ Globals _____ //
            Globals.ContentManager = Content;
            Globals.GraphicsDevice = GraphicsDevice;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Globals.Camera = new OrthographicCamera(GraphicsDevice);

            // _____ Map System _____ //
            _mapSystem = new MapSystem();
            _player = new Player(_mapSystem, Globals.Camera,Content.Load<Texture2D>("player"));

            _mapSystem._player = _player;
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // _____ Player Update _____ //
            _player.Update();
            
            // _____ Map Update _____ //
            _mapSystem.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Black);
            
            // Then we will render the map and player
            _mapSystem.DrawMap(Globals.Camera.GetViewMatrix());

            #region Debugging
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _showDebug = true;
            }
            
            // Draws text above player, showing it's position
            if (_showDebug)
            {
                Engine.Globals.SpriteBatch.Begin();
                var panelPosition = new Vector2(0, 0);
                var textMiddlePoint = _debugFont.MeasureString("Player position: " + _player.Position);
                var textPosition = new Vector2(300, panelPosition.Y + 30);
                
                Engine.Globals.SpriteBatch.Draw(Content.Load<Texture2D>("Debugging/DB_BG"), panelPosition, Color.White);
                Engine.Globals.SpriteBatch.DrawString(_debugFont, "Player position: " + _player.Position, textPosition, Color.White,
                    0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                Engine.Globals.SpriteBatch.End();
                
            }
            #endregion

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