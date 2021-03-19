#region Imports

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
        // Graphics and window
        public static GraphicsDeviceManager Graphics;
        public static Vector2               VirtualSize = new Vector2(1920, 1080);
        public static BoxingViewportAdapter ViewportAdapter;

        public static MapSystem          MapSystem;
        public static GameTime           GameTime;
        public static OrthographicCamera Camera;
        public static ContentManager     ContentManager;
        public static SpriteBatch        SpriteBatch;
        public static SpriteFont         GuiFont;
        public static bool               DebugMode = false;

        /// <summary>
        ///     The number of layers in the game map, not inclusive of the player layer
        /// </summary>
        public static int LayerCount;
    }
}