using System;
using GrimGame.Character;
using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled.Serialization;
using MonoGame.Extended.ViewportAdapters;

namespace GrimGame.Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Player _player;

        public IsometricCamera _camera;
        public TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        
        // TiledObjectRenderer
        private TiledObjectRenderer _tiledObjectRenderer;
        
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
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800 * 3, 600 * 3);
            _camera = new IsometricCamera(viewportadapter, this);
            
            _camera.OrthographicCamera.LookAt(_map.ObjectLayers[0].Objects[0].Position);
            
            _tiledObjectRenderer = new TiledObjectRenderer(this, _map, _spriteBatch);
            
            _player = new Player(this, Content.Load<Texture2D>("player"));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _camera.OrthographicCamera.Move(new Vector2(0, -5));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _camera.OrthographicCamera.Move(new Vector2(0, 5));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _camera.OrthographicCamera.Move(new Vector2(-5, 0));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _camera.OrthographicCamera.Move(new Vector2(5, 0));
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
            _spriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });

            // Then we will render the map
            _mapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());
            _player.SpawnPlayer();
            _player.Update();
            _tiledObjectRenderer.DrawObjects();

            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}