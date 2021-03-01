using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    public class UiManager
    {
        private readonly PauseMenu _pauseMenu;

        public UiManager(Scene scene)
        {
            _pauseMenu = new PauseMenu(scene);
            InputManager.AddKeyPressHandler(OpenPauseMenu, Keys.Escape);
        }

        public void Update()
        {
            _pauseMenu.Update();
        }

        private void OpenPauseMenu()
        {
            _pauseMenu.IsActive = !_pauseMenu.IsActive;
        }

        public void Draw()
        {
            if (_pauseMenu.IsActive)
                _pauseMenu.Draw();
        }
    }
}