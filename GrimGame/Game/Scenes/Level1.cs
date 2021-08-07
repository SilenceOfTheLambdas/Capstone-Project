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

        /// <summary>
        ///     Stores a list of spawned and un-spawned enemies
        /// </summary>
        private List<Paladin> _enemies;

        private Player      _player;
        private FastRandom? _random;

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
                Speed = 80f,
                RunningSpeed = 90f,
                Enabled = true,
                Active = true,
                MaxHp = 50,
                CurrentHp = 50
            };
            _player.Init();
            _enemies = new List<Paladin>();
            _spawnedList = new List<Paladin>();

            // Add 12 enemies
            _random = new FastRandom();
            for (var i = 0; i < 11; i++)
            {
                var newEnemy = new Paladin(_mapSystem, _player)
                    {Speed = _random.NextSingle(0.4f, 1.6f), Enabled = true, Active = true, MaxHp = 100};
                _enemies.Add(newEnemy);
            }

            UiManager = new UiManager(this);
            UiManager.Init();

            // Init debugger
            GrimDebugger.Player = _player;
            GrimDebugger.MapSystem = _mapSystem;
        }

        public override void Update(GameTime gameTime)
        {
            var updatedList = new List<Paladin>(_spawnedList);
            // Check to see of any of the enemies have been killed
            foreach (var paladin in _spawnedList.ToList().Where(paladin => paladin.CurrentHp <= 0))
                updatedList.Remove(paladin);

            _spawnedList = updatedList;

            // If 3 seconds have passed, and there are less than 3 enemies spawned
            if (_spawnedList.Count <= 3)
            {
                _spawnTimer += gameTime.GetElapsedSeconds();
                if (_spawnTimer >= 3)
                {
                    SpawnEnemy();
                    _spawnTimer -= 3;
                }
            }

            _mapSystem.Update(gameTime);
            UiManager.Update();

            base.Update(gameTime);
        }

        private void SpawnEnemy()
        {
            foreach (var enemy in _enemies.ToList())
            {
                enemy.Init(); // spawn it
                _spawnedList.Add(enemy);
            }
        }

        public override void Draw()
        {
            // Clear the screen
            Globals.Graphics.GraphicsDevice.Clear(Color.Black);

            // Sort the player's index
            PlayerLayerIndexer();

            UiManager.Draw();

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
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                _mapSystem.CurrentIndex = Globals.LayerCount;
            }
            else
            {
                _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 2);
            }
        }
    }
}