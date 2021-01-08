#region Imports

using System;
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
            InputManager.AddKeyPressHandler(GrimDebugger.EnableDebugger, Keys.D0);
        }

        public virtual void Draw(GameTime gameTime) {
            // Draws text above player, showing it's position
            if (Globals.DebugMode)
            {
                GrimDebugger.Draw();
            }
        }

        public bool GetIsSceneLoaded() => _isSceneLoaded;
        public void SetIsSceneLoaded(bool answer) => _isSceneLoaded = answer;
    }
}