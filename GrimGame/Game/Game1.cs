using System;
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
        private SpriteBatch _spriteBatch;

        private IsometricCamera _camera;
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
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
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight += 10;
            _graphics.PreferredBackBufferWidth += 10;
            _graphics.ApplyChanges();

            base.Initialize();
            
            _map = Content.Load<TiledMap>("TestLevel");
            // Create the map renderer
            _mapRenderer = new TiledMapRenderer(GraphicsDevice, _map);
            // If you decided to use the camere, then you could also initialize it here like this
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            _camera = new IsometricCamera(viewportadapter);
            
            _camera.OrthographicCamera.LookAt(_map.ObjectLayers[0].Objects[0].Position.Rotate(1));
            Console.WriteLine(_map.ObjectLayers[0].Objects[0].Position);
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
                _camera.OrthographicCamera.Move(new Vector2(0, -1));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _camera.OrthographicCamera.Move(new Vector2(0, 1));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _camera.OrthographicCamera.Move(new Vector2(-1, 0));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _camera.OrthographicCamera.Move(new Vector2(1, 0));
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

            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}