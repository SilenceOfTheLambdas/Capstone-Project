#region Imports

using System.Collections.Generic;
using System.Linq;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

#endregion

namespace GrimGame.Game.Scenes
{
    internal class Level1 : Scene
    {
        private readonly MapSystem _mapSystem;
        private          Player    _player;
        private          Paladin   _paladin;

        /// <summary>
        /// Stores a list of spawned and un-spawned enemies
        /// </summary>
        private List<Paladin> _enemies;

        private List<Paladin> _spawnedList;

        private float _spawnTimer;

        public Level1(string sceneName, string mapName, MainGame mainGame)
            : base(sceneName, mainGame)
        {
            _mapSystem = new MapSystem(mapName);
        }

        public override void Initialize()
        {
            base.Initialize();

            _player = new Player(_mapSystem, Globals.Camera)
            {
                Speed = 2f,
                RunningSpeed = 3.2f,
                Enabled = true,
                Active = true,
                MaxHp = 100,
                CurrentHp = 100
            };
            _player.Init();
            _enemies = new List<Paladin>();
            _spawnedList = new List<Paladin>();

            // Add 12 enemies
            var random = new FastRandom();
            for (var i = 0; i < 12; i++)
            {
                var newEnemy = new Paladin(_mapSystem, _player)
                    {Speed = random.NextSingle(0.2f, 1.2f), Enabled = true, Active = true, MaxHp = 100};
                _enemies.Add(newEnemy);
            }

            _mapSystem.Player = _player;

            UiManager = new UiManager(this);
            UiManager.Init();

            // Init debugger
            if (Globals.DebugMode)
            {
                GrimDebugger.Player = _player;
                GrimDebugger.MapSystem = _mapSystem;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _spawnTimer += gameTime.GetElapsedSeconds();
            if (_spawnTimer >= 3)
            {
                SpawnEnemy();
                _spawnTimer -= 3;
            }

            _mapSystem?.Update(gameTime);
            UiManager?.Update();

            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            foreach (var enemy in _enemies.ToList())
            {
                enemy.Init(); // spawn it
                _enemies.Remove(enemy); // then remove the enemy
            }
        }

        public override void Draw()
        {
            // Clear the screen
            Globals.Graphics.GraphicsDevice.Clear(Color.Black);

            // Sort the player's index
            PlayerLayerIndexer();

            UiManager?.Draw();

            base.Draw();
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
                _mapSystem?.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                if (_mapSystem != null) _mapSystem.CurrentIndex = Globals.LayerCount;
                _player.Collision = true;
            }
            else
            {
                _mapSystem?.DrawMap(Globals.Camera.GetViewMatrix(), 2);
            }
        }
    }
}