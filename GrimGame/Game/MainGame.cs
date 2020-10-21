#region Imports
using System;
using Autofac;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace GrimGame.Game
{
    public class MainGame : BaseGame
    {
        private Player _player;

        // _____ Map System _____ //
        private MapSystem _mapSystem;

        // _____ Debug _____ //
        private bool _showDebug;
        private SpriteFont _debugFont;
        public GrimDebugger GrimDebugger;

        public MainGame()
        {
        }
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            Globals.ContentManager = Content;
            Globals.Camera = new OrthographicCamera(Globals.Graphics.GraphicsDevice);

            builder.RegisterInstance(new SpriteBatch(Globals.Graphics.GraphicsDevice));
            builder.RegisterInstance(Globals.Camera);
        }

        protected override void Initialize()
        {
            base.Initialize();
            var viewportAdapter = new BoxingViewportAdapter(Window, Globals.Graphics.GraphicsDevice, 1280, 720);
            Globals.Camera = new OrthographicCamera(viewportAdapter);

            // _____ Map System _____ //
            _mapSystem = new MapSystem(this);
            _player = new Player(_mapSystem, Globals.Camera,Content.Load<Texture2D>("player"));
            _player.Init(this);

            _mapSystem.Player = _player;
            
            GrimDebugger = new GrimDebugger(_player, _mapSystem, _debugFont);
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            // _____ Player Update _____ //
            _player.Update(this);
            
            // _____ Map Update _____ //
            _mapSystem.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Black);

            TiledMapTileLayer layer = _mapSystem.Map.GetLayer<TiledMapTileLayer>("Wall_south(AbovePlayer)");
            TiledMapTile? tile = null;

            ushort x = (ushort) (_player.Position.X / 32);
            ushort y = (ushort) (_player.Position.Y / 32);

            layer.TryGetTile(x, y, out tile);
            if (tile.HasValue)
            {
                GrimDebugger.Log("Player collided");
            }

            // Then we will render the map and player
            if (_player.Position.Y >= _mapSystem.Map.ObjectLayers[0].Objects[1].Position.Y)
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                _mapSystem.currentIndex = Globals.LayerCount;
            }
            else
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 3);
            }

            #region Debugging
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _showDebug = true;
            }
            
            // Draws text above player, showing it's position
            if (_showDebug)
            {
                GrimDebugger.Draw();
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