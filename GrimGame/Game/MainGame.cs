using System;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace GrimGame.Game
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private Player _player;
        public OrthographicCamera Camera;
        
        // _____ Map System _____ //
        private MapSystem _mapSystem;

        // _____ Debug _____ //
        private bool _showDebug;
        private SpriteFont _debugFont;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the window size
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            
            // _____ Globals _____ //
            Engine.Globals.ContentManager = this.Content;
            Engine.Globals.GraphicsDevice = this.GraphicsDevice;
        }

        protected override void Initialize()
        {
            base.Initialize();
            var viewportadapter = new DefaultViewportAdapter(GraphicsDevice);
            Camera = new OrthographicCamera(viewportadapter);
            // _____ Map System _____ //
            _mapSystem = new MapSystem();
            _player = new Player(_mapSystem, Camera,Content.Load<Texture2D>("player"));

            _mapSystem._player = _player;
        }

        protected override void LoadContent()
        {
            Engine.Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            
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
            GraphicsDevice.Clear(Color.White);

            // Transform matrix is only needed if you have a camera
            // Setting the sampler state to `new SamplerState { Filter = TextureFilter.Point }` will reduce gaps and odd artifacts when using animated tiles
            Engine.Globals.SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });

            // Then we will render the map
            _mapSystem.DrawMap(Camera.GetViewMatrix());

            // _____ PLayer Draw _____ //
            // Engine.Globals.SpriteBatch.Draw(_player.PlayerSprite, _player.Position, null, Color.White, 0f, Vector2.Zero,
            //     new Vector2(0.5f, 0.5f), SpriteEffects.None, 1f);

            // End the sprite batch
            Engine.Globals.SpriteBatch.End();

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