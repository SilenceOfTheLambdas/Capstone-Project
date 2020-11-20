#region Imports
using GrimGame.Engine;
using GrimGame.Game;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
#endregion

namespace GrimGame.Game
{
    public abstract class Scene
    {
        // Is this scene loaded?
        private bool isSceneLoaded;
        public MainGame mainGame;

        #region Public Variables
        public static ObjectManager ObjectManager;
        public int Index;
        public string Name;
        public bool Loaded;
        public UIManager UIManager;
        public GrimDebugger grimDebugger;
        public bool ShowDebug;
        #endregion

        public Scene(string sceneName, MainGame mainGame) {
            ObjectManager = new ObjectManager();
            Name = sceneName;
            this.mainGame = mainGame;
        }

        public virtual void Initialize() {
            grimDebugger = new GrimDebugger(Globals.GuiFont);
        }

        public abstract void LoadContent();

        public virtual void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                ShowDebug = true;
            }
        }

        public virtual void Draw(GameTime gameTime) {
            // Draws text above player, showing it's position
            if (ShowDebug)
            {
                grimDebugger.Draw();
            }
        }

        public bool GetIsSceneLoaded() => isSceneLoaded;
        public void SetIsSceneLoaded(bool answer) => isSceneLoaded = answer;
    }
}