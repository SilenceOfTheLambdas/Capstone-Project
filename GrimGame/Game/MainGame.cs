#region Imports
using System;
using System.Text.RegularExpressions;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

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
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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

            _mapSystem.Player = _player;
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
            if (_player.Position.Y <= _mapSystem.Map.ObjectLayers[0].Objects[1].Size.Height && _player.Position.Y >= _mapSystem.Map.ObjectLayers[0].Objects[1].Position.Y)
            {
                if (_player.Position.X <= _mapSystem.Map.ObjectLayers[0].Objects[1].Size.Width && _player.Position.X >= _mapSystem.Map.ObjectLayers[0].Objects[1].Position.X)
                    _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
            }
            else
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix());
            }

            #region Debugging
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _showDebug = true;
            }
            
            // Draws text above player, showing it's position
            if (_showDebug)
            {
                Globals.SpriteBatch.Begin();
                var panelPosition = new Vector2(0, 0);
                var textMiddlePoint = _debugFont.MeasureString("Player position: " + _player.Position);
                var textPosition = new Vector2(350, panelPosition.Y + 30);
                
                Globals.SpriteBatch.Draw(Content.Load<Texture2D>("Debugging/DB_BG"), panelPosition, Color.White);
                Globals.SpriteBatch.DrawString(_debugFont, "Player position: " + _player.Position + "\n" + "Player Index: " + _mapSystem.currentIndex, textPosition, Color.White,
                    0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
                Globals.SpriteBatch.End();
                
                Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix());
                Globals.SpriteBatch.DrawPolygon(_mapSystem.Map.ObjectLayers[0].Objects[1].Position,
                    new Polygon(new []
                    {
                        _mapSystem.Map.ObjectLayers[0]
                            .Objects[1]
                            .Position,
                        new Vector2(_mapSystem.Map.ObjectLayers[0]
                                        .Objects[1]
                                        .Position.X +
                                    _mapSystem.Map.ObjectLayers[0]
                                        .Objects[1]
                                        .Size.Width, 
                            _mapSystem.Map.ObjectLayers[0].Objects[1].Position.Y),
                        new Vector2(_mapSystem.Map.ObjectLayers[0].Objects[1].Position.X, _mapSystem.Map.ObjectLayers[0].Objects[1].Position.Y + _mapSystem.Map.ObjectLayers[0].Objects[1].Size.Height), 
                    }), Color.Green , 1f, 0f);
                Globals.SpriteBatch.End();
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