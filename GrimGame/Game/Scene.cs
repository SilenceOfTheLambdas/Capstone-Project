#region Imports
using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GrimGame.Game
{
    public abstract class Scene
    {
        // Is this scene loaded?
        private bool _isSceneLoaded;
        public readonly MainGame MainGame;

        #region Public Variables

        protected static ObjectManager ObjectManager;
        public readonly string Name;
        protected UIManager UiManager;
        protected GrimDebugger GrimDebugger;
        private bool _showDebug;
        
        #endregion

        protected Scene(string sceneName, MainGame mainGame) {
            ObjectManager = new ObjectManager();
            Name = sceneName;
            MainGame = mainGame;
        }

        public virtual void Initialize() {
            GrimDebugger = new GrimDebugger(Globals.GuiFont);
        }

        public abstract void LoadContent();

        public virtual void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                _showDebug = true;
            }
        }

        public virtual void Draw(GameTime gameTime) {
            // Draws text above player, showing it's position
            if (_showDebug)
            {
                GrimDebugger.Draw();
            }
        }

        public bool GetIsSceneLoaded() => _isSceneLoaded;
        public void SetIsSceneLoaded(bool answer) => _isSceneLoaded = answer;
    }
}