using System.Collections.Generic;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GrimGame.Game
{
    /// <summary>
    /// This class is responsible for loading all of the maps, drawing them and managing layers.
    /// </summary>
    public class MapSystem
    {
        /// <summary>
        /// The Tiled map
        /// </summary>
        public TiledMap Map;
        /// <summary>
        /// The renderer for the Tiled map
        /// </summary>
        public TiledMapRenderer MapRenderer;
        /// <summary>
        /// Renders drawable objects onto the map
        /// </summary>
        private TiledObjectRenderer _tiledObjectRenderer;

        /// <summary>
        /// Stores a list of every layer in the map
        /// </summary>
        private List<TiledMapLayer> _layers;

        public Player _player;

        public MapSystem()
        {
            Map = Engine.Globals.ContentManager.Load<TiledMap>("Level1");
            // Create the map renderer
            MapRenderer = new TiledMapRenderer(Engine.Globals.GraphicsDevice, Map);
            _tiledObjectRenderer = new TiledObjectRenderer(Map, Engine.Globals.SpriteBatch);
            _layers = new List<TiledMapLayer>();
            
            foreach (var tiledMapLayer in Map.Layers)
            {
                _layers.Add(tiledMapLayer);
            }
        }

        public void Update(GameTime gameTime)
        {
            MapRenderer.Update(gameTime);
        }

        /// <summary>
        /// Renders the map
        /// </summary>
        public void DrawMap(Matrix viewMatrix)
        {
            foreach (var layer in _layers)
            {
                if (layer.Name != "Player")
                    MapRenderer.Draw(layer, viewMatrix);
            }
            // player
            Engine.Globals.SpriteBatch.Draw(_player.PlayerSprite, _player.Position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(0.5f, 0.5f), SpriteEffects.None, 0f);
            _tiledObjectRenderer.DrawObjects();
        }
    }
}