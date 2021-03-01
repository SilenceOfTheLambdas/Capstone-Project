#region Imports

using System.Linq;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;

#endregion

namespace GrimGame.Game.Scenes
{
    internal class Level1 : Scene
    {
        private readonly MapSystem _mapSystem;
        private          Paladin   _paladin;
        private          Player    _player;

        public Level1(string sceneName, string mapName, MainGame mainGame)
            : base(sceneName, mainGame)
        {
            _mapSystem = new MapSystem(mapName);
            SceneManager.AddScene(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (GetIsSceneLoaded())
            {
                #region Map System

                _player = new Player(_mapSystem, Globals.Camera)
                {
                    Name = "Player 1",
                    Tag = Globals.ObjectTags.Player,
                    Speed = 2f,
                    RunningSpeed = 3.2f,
                    Enabled = true,
                    Active = true,
                    MaxHp = 100,
                    CurrentHp = 100
                };
                _player.Init();

                _paladin = new Paladin(_mapSystem, _player)
                {
                    Name = "Paladin",
                    Tag = Globals.ObjectTags.Enemy,
                    Speed = 1f,
                    Enabled = true,
                    Active = true,
                    MaxHp = 100
                };
                _paladin.Init();

                _mapSystem.Player = _player;

                #endregion

                UiManager = new UiManager(this);

                // Init debugger
                if (Globals.DebugMode)
                {
                    GrimDebugger.Player = _player;
                    GrimDebugger.MapSystem = _mapSystem;
                }
            }

            // Adding objects
            ObjectManager.Add(_paladin);
            ObjectManager.Add(_player);
        }

        public override void Update(GameTime gameTime)
        {
            if (GetIsSceneLoaded())
            {
                _mapSystem.Update(gameTime);

                InputManager.Update();
                UiManager.Update();

                ObjectManager.Update(this, gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (GetIsSceneLoaded())
            {
                // Clear the screen
                Globals.Graphics.GraphicsDevice.Clear(Color.Black);

                // Sort the player's index
                PlayerLayerIndexer();

                UiManager.Draw();
            }

            base.Draw(gameTime);
        }

        private void PlayerLayerIndexer()
        {
            var bumpLayer = false;
            foreach (var _ in MapSystem.CollisionObjects.Where(rectangle =>
                _player.BoxCollider.Bounds.Top >= rectangle.Bottom
                && _player.BoxCollider.Bounds.Top <= rectangle.Bottom + _player.Height / 2
                && _player.BoxCollider.Bounds.Right >= rectangle.Left
                && _player.BoxCollider.Bounds.Left <= rectangle.Right))
                bumpLayer = true;

            if (bumpLayer)
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                _mapSystem.CurrentIndex = Globals.LayerCount;
                _player.Collision = true;
            }
            else
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 2);
            }
        }
    }
}