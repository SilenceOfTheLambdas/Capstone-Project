using System;
using GrimGame.Character;
using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace GrimGame.Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Player _player;

        public OrthographicCamera _camera;
        public TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        
        // TiledObjectRenderer
        private TiledObjectRenderer _tiledObjectRenderer;
        
        // _____ Debug _____ //
        private bool _showDebug = false;
        private SpriteFont debugFont;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set the window size
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight += 10;
            _graphics.PreferredBackBufferWidth += 10;
            _graphics.ApplyChanges();

            base.Initialize();
            
            _map = Content.Load<TiledMap>("Level1");
            // Create the map renderer
            _mapRenderer = new TiledMapRenderer(GraphicsDevice, _map);
            // If you decided to use the camere, then you could also initialize it here like this
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportadapter);

            _tiledObjectRenderer = new TiledObjectRenderer(this, _map, _spriteBatch);
            
            _player = new Player(this, Content.Load<Texture2D>("player"));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            debugFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _showDebug = true;
            }
            
            
            _mapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.White);

            // Transform matrix is only needed if you have a camera
            // Setting the sampler state to `new SamplerState { Filter = TextureFilter.Point }` will reduce gaps and odd artifacts when using animated tiles
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });

            // Then we will render the map
            _mapRenderer.Draw(_camera.GetViewMatrix());

            // _____ PLayer Update _____ //
            _player.Update();
            
            // _____ Draw Objects _____ //
            _tiledObjectRenderer.DrawObjects();

            // End the sprite batch
            _spriteBatch.End();

            #region Debugging
            // Draws text above player, showing it's position
            if (_showDebug)
            {
                _spriteBatch.Begin();
                var textMiddlePoint = debugFont.MeasureString("Player position: " + _player.Position);
                var textPosition = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                
                _spriteBatch.DrawString(debugFont, "Player position: " + _player.Position, textPosition, Color.Red, 
                    0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                _spriteBatch.End();
                
            }
            #endregion

            base.Draw(gameTime);
        }
    }
}