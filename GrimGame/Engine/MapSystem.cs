#region Imports

using System.Collections.Generic;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

#endregion

namespace GrimGame.Engine
{
    /// <summary>
    ///     This class is responsible for loading all of the maps, drawing them and managing layers.
    /// </summary>
    public class MapSystem
    {
        /// <summary>
        ///     The renderer for the Tiled map
        /// </summary>
        private readonly TiledMapRenderer _mapRenderer;

        /// <summary>
        ///     Renders drawable objects onto the map
        /// </summary>
        private readonly TiledObjectRenderer _tiledObjectRenderer;

        /// <summary>
        ///     A dictionary storing the collision layers.
        ///     Rectangle: The collision bounds
        ///     bool: Is this collision bound below the player?
        /// </summary>
        public readonly List<Rectangle> CollisionObjects = new List<Rectangle>();

        public readonly Dictionary<Rectangle, bool> FrontAndBackWalls = new Dictionary<Rectangle, bool>();

        /// <summary>
        ///     The Tiled map
        /// </summary>
        public readonly TiledMap Map;

        public int CurrentIndex;

        public Player Player;

        public MapSystem(string mapName)
        {
            Map = Globals.ContentManager.Load<TiledMap>(mapName);
            Globals.LayerCount = Map.Layers.Count - 1;
            // Create the map renderer
            _mapRenderer = new TiledMapRenderer(Globals.Graphics.GraphicsDevice, Map);
            _tiledObjectRenderer = new TiledObjectRenderer(Map, Globals.SpriteBatch);
            RenderQueue = new Dictionary<int, TiledMapLayer>();

            // First add all of the layers below the player
            for (var i = 0; i < Map.Layers.Count; i++) RenderQueue.Add(i, Map.Layers[i]);

            foreach (var (key, value) in RenderQueue)
            {
                if (!value.Name.Equals("Player")) continue;
                CurrentIndex = key;
            }

            // Adding collision objects
            foreach (var o in Map.ObjectLayers[2].Objects)
                CollisionObjects.Add(new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width,
                    (int) o.Size.Height));

            // Adding front wall collision objects
            foreach (var o in Map.ObjectLayers[0].Objects)
            {
                FrontAndBackWalls.Add(
                    new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width, (int) o.Size.Height),
                    true);
                CollisionObjects.Add(new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width,
                    (int) o.Size.Height));
            }

            foreach (var o in Map.ObjectLayers[3].Objects)
            {
                FrontAndBackWalls.Add(
                    new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width, (int) o.Size.Height),
                    false);
                CollisionObjects.Add(new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width,
                    (int) o.Size.Height));
            }

            foreach (var o in Map.ObjectLayers[1].Objects)
            {
                FrontAndBackWalls.Add(
                    new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width, (int) o.Size.Height),
                    false);
                CollisionObjects.Add(new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width,
                    (int) o.Size.Height));
            }
        }

        /// <summary>
        ///     Stores a dictionary attaining to the render order of the map.
        ///     The player will be on a layer, this index is stored in <code>playerIndex</code>.
        ///     int: an index that defines the order on which a layer is drawn
        ///     TiledMapLayer: the layer associated with that index
        /// </summary>
        private Dictionary<int, TiledMapLayer> RenderQueue { get; }

        public void Update(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }

        /// <summary>
        ///     Draws the map, but instead allows for a parameter that specifies the player's index.
        /// </summary>
        /// <param name="viewMatrix">The camera's view matrix.</param>
        /// <param name="newPlayerIndex">The new layer index of the player sprite.</param>
        public void DrawMap(Matrix viewMatrix, int newPlayerIndex)
        {
            CurrentIndex = newPlayerIndex;
            // Below player
            for (var i = 0; i < newPlayerIndex; i++) _mapRenderer.Draw(RenderQueue[i], viewMatrix);

            if (Player.Enabled)
                // The player
                Player.Draw();

            // Above player
            for (var i = newPlayerIndex + 1; i < RenderQueue.Count; i++) _mapRenderer.Draw(RenderQueue[i], viewMatrix);

            // Draw any objects that are visible in-game
            _tiledObjectRenderer.DrawObjects();
        }
    }
}