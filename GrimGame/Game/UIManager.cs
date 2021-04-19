using System;
using System.Linq;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Game
{
    /// <summary>
    ///     Manages the UI for the game.
    /// </summary>
    public class UiManager
    {
        private static   EndGameMenu _endGameMenu;
        private readonly PauseMenu   _pauseMenu;
        private readonly PlayerHud   _playerHud;
        private readonly Shop        _shop;

        public UiManager(Scene scene)
        {
            _pauseMenu = new PauseMenu(scene);
            _playerHud = new PlayerHud();
            _shop = new Shop(SceneManager.GetActiveScene.ObjectManager.Objects.First(o => o is Player) as Player ??
                             throw new InvalidOperationException());
            _endGameMenu = new EndGameMenu(scene);
            InputManager.AddKeyPressHandler(OpenPauseMenu, Keys.Escape);
            InputManager.AddKeyPressHandler(OpenShop, Keys.P);
        }

        private void OpenShop()
        {
            _shop.IsActive = !_shop.IsActive;
        }

        public void Init()
        {
            _playerHud.Init();
        }

        public void Update()
        {
            _pauseMenu.Update();
            _playerHud.Update();
            _shop.Update();
            if (_endGameMenu.IsActive)
                _endGameMenu.Update();
        }

        private void OpenPauseMenu()
        {
            _pauseMenu.IsActive = !_pauseMenu.IsActive;
        }

        public static void DisplayEndScreen()
        {
            _endGameMenu.IsActive = true;
        }

        public void Draw()
        {
            if (_pauseMenu.IsActive)
                _pauseMenu.Draw();
            if (PlayerHud.IsActive)
                _playerHud.Draw();
            if (_shop.IsActive)
                _shop.Draw();
            if (_endGameMenu.IsActive)
                _endGameMenu.Draw();
        }
    }
}