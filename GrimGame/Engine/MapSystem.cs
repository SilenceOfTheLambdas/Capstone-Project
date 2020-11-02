#region Imports
using System.Collections.Generic;
using GrimGame.Game;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
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
        private MainGame _game;
        
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
        private Dictionary<int, TiledMapLayer> RenderQueue {  get;  set; }

        /// <summary>
        /// A dictionary storing the collision layers.
        /// Rectangle: The collision bounds
        /// bool: Is this collision bound below the player?
        /// </summary>
        public Dictionary<Rectangle, bool> CollisionObjects = new Dictionary<Rectangle, bool>();

        private readonly int _playerIndex;
        public int currentIndex;

        public Player Player;

        public MapSystem(MainGame g)
        {
            this._game = g;
            Map = Globals.ContentManager.Load<TiledMap>("TestMap");
            Globals.LayerCount = Map.Layers.Count - 1;
            // Create the map renderer
            MapRenderer = new TiledMapRenderer(Globals.Graphics.GraphicsDevice, Map);
            _tiledObjectRenderer = new TiledObjectRenderer(Map, Globals.SpriteBatch);
            RenderQueue = new Dictionary<int, TiledMapLayer>();

            // First add all of the layers below the player
            for (var i = 0; i < Map.Layers.Count; i++)
            {
                RenderQueue.Add(i, Map.Layers[i]);
            }

            foreach (var (key, value) in RenderQueue)
            {
                if (!value.Name.Equals("Player")) continue;
                _playerIndex = key;
                currentIndex = key;
            }

            foreach (var o in Map.ObjectLayers[1].Objects)
            {
                CollisionObjects.Add(new Rectangle((int)o.Position.X, (int)o.Position.Y, (int)o.Size.Width, (int)o.Size.Height), false);
            }

            foreach (var o in Map.ObjectLayers[2].Objects)
            {
                CollisionObjects.Add(new Rectangle((int)o.Position.X, (int)o.Position.Y, (int)o.Size.Width, (int)o.Size.Height), true);
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
                MapRenderer.Draw(RenderQueue[i], viewMatrix);
            }
            
            // The player
            Player.Draw(_game);
            
            // Above player
            for (var i = _playerIndex + 1; i < RenderQueue.Count; i++)
            {
                MapRenderer.Draw(RenderQueue[i], viewMatrix);
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
                MapRenderer.Draw(RenderQueue[i], viewMatrix);
            }
            
            // The player
            Player.Draw(_game);
            
            if (_game.ShowDebug)
            {
                //_game.Debugger.DrawGrid();
            }
            
            // Above player
            for (var i = newPlayerIndex + 1; i < RenderQueue.Count; i++)
            {
                MapRenderer.Draw(RenderQueue[i], viewMatrix);
            }

            // Draw any objects that are visible in-game
            _tiledObjectRenderer.DrawObjects();
        }
        
    }
}