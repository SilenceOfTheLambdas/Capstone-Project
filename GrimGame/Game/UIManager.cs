using System.Diagnostics;
using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Game
{
    public class UIManager
    {
        private readonly PauseMenu _pauseMenu;
        
        public UIManager(Scene scene)
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