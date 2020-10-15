#region Imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
#endregion

namespace GrimGame.Engine
{
    public static class Globals
    {
        public static GameTime GameTime;
        public static OrthographicCamera Camera;
        public static ContentManager ContentManager;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDevice GraphicsDevice;
        /// <summary>
        /// The number of layers in the game map, not inclusive of the player layer
        /// </summary>
        public static int LayerCount;
    }
}