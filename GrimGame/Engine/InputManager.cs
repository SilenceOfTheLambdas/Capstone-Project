using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Engine
{
    public static class InputManager
    {
        private static KeyboardState _ks, _ksOld = Keyboard.GetState();
        
        private static readonly Dictionary<Keys, List<Action>> OnKeyPress = new Dictionary<Keys, List<Action>>();

        public static void AddKeyPressHandler(Action handler, Keys key)
        {
            if (!OnKeyPress.ContainsKey(key))
                OnKeyPress.Add(key, new List<Action>());
            OnKeyPress[key].Add(handler);
        }

        public static void Update()
        {
            _ks = Keyboard.GetState();

            foreach (Keys key in Enum.GetValues(typeof(Keys))) 
            {
                if (!_ks.IsKeyDown(key) && _ksOld.IsKeyDown(key) && OnKeyPress.ContainsKey(key))
                    foreach (var handler in OnKeyPress[key])
                        handler();
            }

            _ksOld = _ks;
        }
    }
}