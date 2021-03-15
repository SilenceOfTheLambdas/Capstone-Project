using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    /// <summary>
    ///     Manages the UI for the game.
    /// </summary>
    public class UiManager
    {
        private readonly PauseMenu _pauseMenu;
        private readonly PlayerHud _playerHud;

        public UiManager(Scene scene)
        {
            _pauseMenu = new PauseMenu(scene);
            _playerHud = new PlayerHud();
            InputManager.AddKeyPressHandler(OpenPauseMenu, Keys.Escape);
        }

        public void Init()
        {
            _playerHud.Init();
        }

        public void Update()
        {
            _pauseMenu.Update();
            _playerHud.Update();
        }

        private void OpenPauseMenu()
        {
            _pauseMenu.IsActive = !_pauseMenu.IsActive;
        }

        public void Draw()
        {
            if (_pauseMenu.IsActive)
                _pauseMenu.Draw();
            if (PlayerHud.IsActive)
                _playerHud.Draw();
        }
    }
}