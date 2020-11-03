#region Imports
using System;
using Autofac;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Extensions;
using MonoGame.Extended;
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
        public bool ShowDebug;
        private SpriteFont _debugFont;
        private GrimDebugger _debugger;

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
            _player = new Player(_mapSystem, Globals.Camera)
            {
                Name = "Player 1",
                Tag = Globals.ObjectTags.Player,
                Speed = 2f,
                RunningSpeed = 3.2f
            };
            _player.Init(this);

            _mapSystem.Player = _player;
            
            _debugger = new GrimDebugger(_player, _mapSystem, _debugFont);
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

            #region Player Layer Index
            foreach (var (rectangle, isBelowPlayer) in _mapSystem.FrontAndBackWalls)
            {
                if (isBelowPlayer)
                {
                    if (_player.BoxCollider.Bounds.Top >= rectangle.Bottom)
                    {
                        _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                        _mapSystem.CurrentIndex = Globals.LayerCount;
                    }
                    else
                    {
                        _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 3);
                    }
                }
            }
            #endregion

            #region Debugging
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                ShowDebug = true;
            }
            
            // Draws text above player, showing it's position
            if (ShowDebug)
            {
                _debugger.Draw();
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