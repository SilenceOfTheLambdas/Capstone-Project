using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Engine
{
    /// <summary>
    ///     Manages the various inputs and events.
    /// </summary>
    public static class InputManager
    {
        /// <summary>
        ///     The current <see cref="KeyboardState" />
        /// </summary>
        private static KeyboardState _ks;

        /// <summary>
        ///     The original <see cref="KeyboardState" />
        /// </summary>
        private static KeyboardState _ksOld = Keyboard.GetState();

        /// <summary>
        ///     A list of keys and their associated actions.
        /// </summary>
        private static readonly Dictionary<Keys, List<Action>> OnKeyPress = new Dictionary<Keys, List<Action>>();

        /// <summary>
        ///     Add a new <see cref="Action" /> to a given key when pressed.
        /// </summary>
        /// <param name="handler">The action to perform; a delegate function</param>
        /// <param name="key">The key that needs to be pressed to active the action</param>
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
                if (!_ks.IsKeyDown(key) && _ksOld.IsKeyDown(key) && OnKeyPress.ContainsKey(key))
                    foreach (var handler in OnKeyPress[key])
                        handler();

            _ksOld = _ks;
        }
    }
}