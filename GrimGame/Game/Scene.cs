#region Imports

using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace GrimGame.Game
{
    /// <summary>
    ///     A scene is an abstract representation of a game level. Every scene will be comprised of many game objects,
    ///     along with UI, and other logic.
    ///     <seealso cref="SceneManager" />
    /// </summary>
    public abstract class Scene
    {
        public readonly MainGame MainGame;

        // Is this scene loaded?
        private bool _isSceneLoaded;

        /// <summary>
        ///     Represents a level. A level may contain various <see cref="GameObject" />s.
        /// </summary>
        /// <param name="sceneName">The name of the Scene. E.e "main", "Level1"</param>
        /// <param name="mainGame">A reference to the main game.</param>
        protected Scene(string sceneName, MainGame mainGame)
        {
            ObjectManager = new ObjectManager();
            Name = sceneName;
            MainGame = mainGame;
            SceneManager.AddScene(this);
        }

        public virtual void Initialize()
        {
            GrimDebugger = new GrimDebugger(Globals.GuiFont);
        }

        /// <summary>
        /// Only called when this scene is active
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            ObjectManager.Update(gameTime);
            InputManager.Update();
            InputManager.AddKeyPressHandler(GrimDebugger.EnableDebugger, Keys.D0);
        }

        public virtual void Draw()
        {
            // Draws text above player, showing it's position
            if (Globals.DebugMode) GrimDebugger.Draw();
        }

        public bool GetIsSceneLoaded()
        {
            return _isSceneLoaded;
        }

        public void SetIsSceneLoaded(bool answer)
        {
            _isSceneLoaded = answer;
        }

        #region Public Variables

        public readonly ObjectManager ObjectManager;
        public readonly string        Name;
        protected       UiManager     UiManager;
        protected       GrimDebugger  GrimDebugger;

        #endregion
    }
}