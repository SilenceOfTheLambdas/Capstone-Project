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
        // _____ Screen _____ //
        private const int Width = 1280; // Width of the window
        private const int Height = 720; // Height of the window

        // _____ Map System _____ //
        private MapSystem _mapSystem;

        // _____ Debug _____ //
        public bool ShowDebug;
        private SpriteFont _debugFont;
        
        // _____ Objects _____ //
        public static ObjectManager ObjectManager;
        private GrimDebugger _debugger;
        private Player _player;

        private UIManager _uiManager;

        public MainGame()
        {
            Globals.Graphics = new GraphicsDeviceManager(this);
            
            IsMouseVisible = true;
            Content.RootDirectory = "Content";

            Globals.Graphics.PreferredBackBufferWidth = Width;
            Globals.Graphics.PreferredBackBufferHeight = Height;
            Globals.Graphics.IsFullScreen = true;
            Globals.Graphics.ApplyChanges();
            
            ObjectManager = new ObjectManager();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            Globals.ContentManager = Content;
            Globals.GameTime = new GameTime();
            Globals.Camera = new OrthographicCamera(Globals.Graphics.GraphicsDevice);
            Window.Title = "GrimDoom";
            var viewportAdapter = new DefaultViewportAdapter(Globals.Graphics.GraphicsDevice);
            Globals.Camera = new OrthographicCamera(viewportAdapter);

            // _____ Map System _____ //
            _mapSystem = new MapSystem(this);
            _player = new Player(_mapSystem, Globals.Camera)
            {
                Name = "Player 1",
                Tag = Globals.ObjectTags.Player,
                Speed = 2f,
                RunningSpeed = 3.2f,
                Enabled = true,
                Active = true
            };
            _player.Init(this);

            _mapSystem.Player = _player;
            
            _debugger = new GrimDebugger(_player, _mapSystem, _debugFont);
            
            _uiManager = new UIManager(this);
            
            // Add game objects to object list
            ObjectManager.Add(_player, this);
        }

        protected override void LoadContent()
        {
            Globals.SpriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);
            _debugFont = Content.Load<SpriteFont>("Fonts/debugFont");
            Globals.GuiFont = Content.Load<SpriteFont>("Fonts/debugFont");
        }

        protected override void Update(GameTime gameTime)
        {
            // _____ Player Update _____ //
            if (_player.Active)
                _player.Update(this);
            
            // _____ Map Update _____ //
            _mapSystem.Update(gameTime);

            // InputManager Update
            InputManager.Update();
            
            _uiManager.Update();

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
            
            _uiManager.Draw();

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