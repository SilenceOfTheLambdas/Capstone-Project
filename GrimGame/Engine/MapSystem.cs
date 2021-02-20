#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using GrimGame.Character.Enemies.AI;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using MLEM.Extended.Tiled;
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
        ///     A dictionary storing the collision layers.
        ///     Rectangle: The collision bounds
        ///     bool: Is this collision bound below the player?
        /// </summary>
        public static readonly List<Rectangle> CollisionObjects = new List<Rectangle>();

        /// <summary>
        ///     The renderer for the Tiled map
        /// </summary>
        private readonly TiledMapRenderer _mapRenderer;

        /// <summary>
        ///     Renders drawable objects onto the map
        /// </summary>
        private readonly TiledObjectRenderer _tiledObjectRenderer;

        /// <summary>
        ///     The Tiled map
        /// </summary>
        public readonly TiledMap Map;

        public int CurrentIndex;

        public Player Player;

        public Node StartNode { get; private set; }

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

            #region Adding Collision Objects

            // Add all of the collision objects to the CollisionObjects list
            foreach (var o in Map.ObjectLayers[0].Objects)
                CollisionObjects.Add(new Rectangle((int) o.Position.X, (int) o.Position.Y, (int) o.Size.Width,
                    (int) o.Size.Height));

            #endregion
            
            // Add map system to Globals
            Globals.MapSystem = this;

            StartNode = new Node()
            {
                
            }
        }

        /// <summary>
        ///     Stores a dictionary pertaining to the render order of the map.
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
            for (var i = 0; i <= newPlayerIndex; i++) _mapRenderer.Draw(RenderQueue[i], viewMatrix);

            if (Player.Enabled)
                // The player
                Player.Draw();

            // Above player
            if (newPlayerIndex < RenderQueue.Count)
                for (var i = newPlayerIndex + 1; i < RenderQueue.Count; i++)
                    _mapRenderer.Draw(RenderQueue[i], viewMatrix);

            // Draw any objects that are visible in-game
            _tiledObjectRenderer.DrawObjects();
        }

        public void MapToWorld(int column, int row, bool centered)
        {
            var screenPosition = new Vector2();

            // TODO: inmap
        }

        /// <summary>
        /// Gets weather the tile at a given location has a collision box above it.
        /// </summary>
        /// <param name="x">X position of the tile</param>
        /// <param name="y">Y position of the tile</param>
        /// <returns>Does this tile have a collider?</returns>
        public bool IsTileCollision(int x, int y)
        {
            if (CollisionObjects.FirstOrDefault(c =>
                Map.GetTiles(x, y).ToList().FirstOrDefault().X == c.X && Map.GetTile("Ground_1", x, y).Y == c.Y).X == x)
            {
                if (CollisionObjects.FirstOrDefault(c => Map.GetTile("Ground_1", x, y).X == c.X && Map.GetTile("Ground_1", x, y).Y == c.Y).Y == y)
                {
                    return true;
                }
            }

            return false;
        }

        private bool InMap(int column, int row)
        {
            return (row >= 0 && row < Map.TileHeight
            && column >= 0 && column < Map.TileWidth);
        }
    }

    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Point Point { get; set; }
        public List<Edge> Connections { get; set; } = new List<Edge>();

        public double? MinCostToStart { get; set; }
        public Node NearestToStart { get; set; }
        public bool Visited { get; set; }
        public double StraightLineDistanceToEnd { get; set; }

        internal static Node GetRandom(Random rnd, string name)
        {
            return new Node
            {
                Point = new Point
                {
                    X = (int) rnd.NextDouble(),
                    Y = (int) rnd.NextDouble()
                },
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        internal void ConnectClosestNodes(List<Node> nodes, int branching, Random rnd, bool randomWeight)
        {
            var connections = new List<Edge>();
            foreach (var node in nodes)
            {
                if (node.Id == this.Id)
                    continue;

                var dist = Math.Sqrt(Math.Pow(Point.X - node.Point.X, 2) + Math.Pow(Point.Y - node.Point.Y, 2));
                connections.Add(new Edge
                {
                    ConnectedNode = node,
                    Length = dist,
                    Cost = randomWeight ? rnd.NextDouble() : dist,
                });
            }

            connections = connections.OrderBy(x => x.Length).ToList();
            var count = 0;
            foreach (var cnn in connections)
            {
                //Connect three closes nodes that are not connected.
                if (!Connections.Any(c => c.ConnectedNode == cnn.ConnectedNode))
                    Connections.Add(cnn);
                count++;

                //Make it a two way connection if not already connected
                if (!cnn.ConnectedNode.Connections.Any(cc => cc.ConnectedNode == this))
                {
                    var backConnection = new Edge {ConnectedNode = this, Length = cnn.Length};
                    cnn.ConnectedNode.Connections.Add(backConnection);
                }

                if (count == branching)
                    return;
            }
        }
    }
    
    public class Edge {
        public double Length { get; set; }
        public double Cost { get; set; }
        public Node ConnectedNode { get; set; }

        public override string ToString()
        {
            return "-> " + ConnectedNode.ToString();
        }
    }
}