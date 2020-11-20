#region Imports
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
#endregion

namespace GrimGame.Game.Levels
{
    class Level1 : Scene
    {

        public MapSystem MapSystem;
        public Player Player;

        public Level1(string sceneName, string mapName, MainGame mainGame) 
        : base(sceneName, mainGame)
        {
            MapSystem = new MapSystem(mapName);
            SceneManager.AddScene(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (GetIsSceneLoaded()) {
                #region Map System
                Player = new Player(MapSystem, Globals.Camera)
                {
                    Name = "Player 1",
                    Tag = Globals.ObjectTags.Player,
                    Speed = 2f,
                    RunningSpeed = 3.2f,
                    Enabled = true,
                    Active = true
                };
                Player.Init();

                MapSystem.Player = Player;
                #endregion

                UIManager = new UIManager(this);

                // Init debugger
                grimDebugger.Player = Player;
                grimDebugger.MapSystem = MapSystem;
            }
        }

        public override void LoadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (GetIsSceneLoaded()) {
                if (Player.Active)
                Player.Update(this);

                MapSystem.Update(gameTime);

                InputManager.Update();
                UIManager.Update();

                ObjectManager.Update(this);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (GetIsSceneLoaded()) {
                // Clear the screen
                Globals.Graphics.GraphicsDevice.Clear(Color.Black);

                // Sort the player's index
                PlayerLayerIndexer();

                UIManager.Draw();
            }

            base.Draw(gameTime);
        }

        private void PlayerLayerIndexer() {
            foreach (var (rectangle, isBelowPlayer) in MapSystem.FrontAndBackWalls)
            {
                if (isBelowPlayer)
                {
                    if (Player.BoxCollider.Bounds.Top >= rectangle.Bottom)
                    {
                        MapSystem.DrawMap(Globals.Camera.GetViewMatrix(), Globals.LayerCount);
                        MapSystem.CurrentIndex = Globals.LayerCount;
                    }
                    else
                    {
                        MapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 3);
                    }
                }
            }
        }
    }
}