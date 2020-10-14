#region Imports
using System;
using System.Collections.Generic;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
#endregion

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

        private List<TiledMapLayer> _layersAbovePlayer;

        public Player _player;

        public MapSystem()
        {
            Map = Globals.ContentManager.Load<TiledMap>("Level1");
            // Create the map renderer
            MapRenderer = new TiledMapRenderer(Globals.GraphicsDevice, Map);
            _tiledObjectRenderer = new TiledObjectRenderer(Map, Globals.SpriteBatch);
            _layers = new List<TiledMapLayer>();
            _layersAbovePlayer = new List<TiledMapLayer>();

            for (var i = 0; i < Map.Layers.Count; i++)
            {
                if (Map.Layers[i].Name.ToLower().Contains("aboveplayer"))
                {
                    _layersAbovePlayer.Add(Map.Layers[i]);
                }
            }

            for (int i = 0; i < Map.Layers.Count; i++)
            {
                if (!Map.Layers[i].Name.ToLower().Contains("aboveplayer"))
                {
                    _layers.Add(Map.Layers[i]);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            MapRenderer.Update(gameTime);
        }

        /// <summary>
        /// Renders the map
        /// </summary>
        public void DrawMap(Matrix? viewMatrix)
        {
            Globals.SpriteBatch.Begin(samplerState: new SamplerState { Filter = TextureFilter.Point });
            for (int i = 0; i < _layers.Count; i++)
            {
                MapRenderer.Draw(_layers[i], viewMatrix);
            }
            // Drawing of player sprite
            Globals.SpriteBatch.Draw(_player.PlayerSprite, _player.Position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.1f);
            Globals.SpriteBatch.End();
            
            // Drawing of obscured parts of the map
            
            // Make new spriteBatch
            Globals.SpriteBatch.Begin(samplerState: new SamplerState { Filter = TextureFilter.Point });
            // South Wall layer
            foreach (var layer in _layersAbovePlayer)
            {
                MapRenderer.Draw(layer, viewMatrix);
            }
            // End spriteBatch
            Globals.SpriteBatch.End();
            
            _tiledObjectRenderer.DrawObjects();
        }
        
    }
}