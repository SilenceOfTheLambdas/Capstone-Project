#region Imports

using System.Collections.Generic;
using Dcrew.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
#endregion

namespace GrimGame.Engine
{
    public static class Globals
    {
        /// <summary>
        /// The list of all active game objects.
        /// </summary>
        public static List<GameObject> GameObjects { get; set; }
        public static GameTime GameTime;
        public static OrthographicCamera Camera;
        public static ContentManager ContentManager;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        /// <summary>
        /// The number of layers in the game map, not inclusive of the player layer
        /// </summary>
        public static int LayerCount;

        public enum ObjectTags
        {
               Player,
               MainCamera,
               Enemy
        }
    }
}