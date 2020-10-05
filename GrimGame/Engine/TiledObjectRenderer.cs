using System;
using System.Collections.Generic;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled;

namespace GrimGame.Engine
{
    public class TiledObjectRenderer
    {
        private TiledMap _map;
        private SpriteBatch _spriteBatch;

        public TiledObjectRenderer(TiledMap map, SpriteBatch spriteBatch)
        {
            _map = map;
            _spriteBatch = spriteBatch;
        }

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