#region Imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
#endregion

namespace GrimGame.Engine
{
    /// <summary>
    /// Renders objects from Tiled into the game world.
    /// </summary>
    public class TiledObjectRenderer
    {
        private readonly TiledMap _map;
        private readonly SpriteBatch _spriteBatch;

        public TiledObjectRenderer(TiledMap map, SpriteBatch spriteBatch)
        {
            _map = map;
            _spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Draws the object into the game world.
        /// </summary>
        public void DrawObjects()
        {

            foreach (var objectLayer in _map.ObjectLayers)
            {
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (!layerObject.Type.ToLower().Equals("nodraw"))
                        _spriteBatch.Draw(Globals.ContentManager.Load<Texture2D>("Objects/" + layerObject.Name),
                            layerObject.Position,
                            null,
                            Color.White,
                            0,
                            Vector2.Zero,
                            new Vector2(1f, 1f),
                            layerObject.Type == "Flipped_horizontal" ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            0);
                }
            }
            
        }
    }
}