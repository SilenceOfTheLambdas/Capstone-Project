using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    public class UiManager
    {
        private readonly PauseMenu _pauseMenu;
        private readonly PlayerHud _playerHud;

        public UiManager(Scene scene)
        {
            _pauseMenu = new PauseMenu(scene);
            _playerHud = new PlayerHud(scene);
            InputManager.AddKeyPressHandler(OpenPauseMenu, Keys.Escape);
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
            if (_playerHud.IsActive)
                _playerHud.Draw();
        }
    }
}