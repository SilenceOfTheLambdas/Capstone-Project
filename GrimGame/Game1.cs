using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace MonoGameCross_PlatformDesktopApplication
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private OrthographicCamera _camera;
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
            _camera = new OrthographicCamera(viewportadapter);
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
            _mapRenderer.Update(gameTime);
            _camera.LookAt(new Vector2(0, 0));

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

            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}