using GrimGame.Engine;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace GrimGame.Game
{
    public class UIManager
    {
        public PauseMenu PauseMenu;
        private float _keyDelay = 1f;
        private float _timePassed = 0f;
        
        public UIManager(MainGame game)
        {
            PauseMenu = new PauseMenu(game);
        }

        public void InputManager()
        {
            _timePassed += (float) Globals.GameTime.GetElapsedSeconds();
            
            var pause = Keys.Escape;
            if (Keyboard.GetState().IsKeyDown(pause))
            {
                PauseMenu.IsActive = !PauseMenu.IsActive;
                _timePassed = 0f;
            }
        }

        public void Update()
        {
            InputManager();
            
            PauseMenu.Update();
        }

        public void Draw()
        {
            if (PauseMenu.IsActive)
                PauseMenu.Draw();
        }
    }
}