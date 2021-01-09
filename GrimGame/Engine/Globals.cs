#region Imports

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace GrimGame.Engine
{
    public static class Globals
    {
        public enum ObjectTags
        {
            Player,
            MainCamera,
            Enemy
        }

        // Graphics and window
        public static GraphicsDeviceManager Graphics;
        public static GameWindow            GameWindow;
        public static Vector2               WindowSize  = new Vector2(720, 480);
        public static Vector2               VirtualSize = new Vector2(1920, 1080);
        public static BoxingViewportAdapter ViewportAdapter;

        public static GameTime           GameTime;
        public static OrthographicCamera Camera;
        public static ContentManager     ContentManager;
        public static SpriteBatch        SpriteBatch;
        public static SpriteFont         GuiFont;
        public static bool               DebugMode = true;
        public static SpriteFont         DebugFont;

        /// <summary>
        ///     The number of layers in the game map, not inclusive of the player layer
        /// </summary>
        public static int LayerCount;

        /// <summary>
        ///     The list of all active game objects.
        /// </summary>
        public static List<GameObject> GameObjects { get; set; }
    }
}