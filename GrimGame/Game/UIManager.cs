using System.Diagnostics;
using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Game
{
    public class UIManager
    {
        private PauseMenu _pauseMenu;
        Stopwatch sw = new Stopwatch();
        
        public UIManager(MainGame game)
        {
            _pauseMenu = new PauseMenu(game);
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