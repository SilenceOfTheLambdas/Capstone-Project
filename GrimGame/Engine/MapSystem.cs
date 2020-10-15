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

namespace GrimGame.Engine
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
        /// Stores a dictionary attaining to the render order of the map.
        /// The player will be on a layer, this index is stored in <code>playerIndex</code>.
        /// 
        /// int: an index that defines the order on which a layer is drawn
        /// TiledMapLayer: the layer associated with that index
        /// </summary>
        private Dictionary<int, TiledMapLayer> RenderOrder {  get;  set; }

        private readonly int _playerIndex;
        public int currentIndex;

        public Player Player;

        public MapSystem()
        {
            Map = Globals.ContentManager.Load<TiledMap>("TestMap");
            Globals.LayerCount = Map.Layers.Count - 1;
            // Create the map renderer
            MapRenderer = new TiledMapRenderer(Globals.GraphicsDevice, Map);
            _tiledObjectRenderer = new TiledObjectRenderer(Map, Globals.SpriteBatch);
            RenderOrder = new Dictionary<int, TiledMapLayer>();

            // First add all of the layers below the player
            for (var i = 0; i < Map.Layers.Count; i++)
            {
                RenderOrder.Add(i, Map.Layers[i]);
            }

            foreach (var (key, value) in RenderOrder)
            {
                if (value.Name.Equals("Player"))
                {
                    _playerIndex = key;
                    currentIndex = key;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            MapRenderer.Update(gameTime);
        }

        /// <summary>
        /// Renders the map and makes sure layers are sorted correctly.
        /// </summary>
        /// <param name="viewMatrix">The Camera's view matrix</param>
        public void DrawMap(Matrix viewMatrix)
        {
            // Below player
            for (var i = 0; i < _playerIndex; i++)
            {
                MapRenderer.Draw(RenderOrder[i], viewMatrix);
            }
            
            // The player
            Player.Draw();
            
            // Above player
            for (var i = _playerIndex + 1; i < RenderOrder.Count; i++)
            {
                MapRenderer.Draw(RenderOrder[i], viewMatrix);
            }

            // Draw any objects that are visible in-game
            _tiledObjectRenderer.DrawObjects();
        }

        /// <summary>
        /// Draws the map, but instead allows for a parameter that specifies the player's index.
        /// </summary>
        /// <param name="viewMatrix">The camera's view matrix.</param>
        /// <param name="newPlayerIndex">The new layer index of the player sprite.</param>
        public void DrawMap(Matrix viewMatrix, int newPlayerIndex)
        {
            currentIndex = newPlayerIndex;
            // Below player
            for (var i = 0; i < newPlayerIndex; i++)
            {
                MapRenderer.Draw(RenderOrder[i], viewMatrix);
            }
            
            // The player
            Player.Draw();
            
            // Above player
            for (var i = newPlayerIndex + 1; i < RenderOrder.Count; i++)
            {
                MapRenderer.Draw(RenderOrder[i], viewMatrix);
            }

            // Draw any objects that are visible in-game
            _tiledObjectRenderer.DrawObjects();
        }
        
    }
}