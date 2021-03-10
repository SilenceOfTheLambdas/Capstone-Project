using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MLEM.Extended.Tiled;
using MonoGame.Extended.Tiled;

namespace GrimGame.Character.Enemies.AI
{
    /// <summary>
    ///     Represents one node in the search space
    /// </summary>
    public class SearchNode
    {
        /// <summary>
        ///     The approximate distance from the start node to the
        ///     goal node if the path goes through this node. (F)
        /// </summary>
        public float DistanceToGoal;

        /// <summary>
        ///     Distance traveled from the spawn point. (G)
        /// </summary>
        public float DistanceTraveled;

        /// <summary>
        ///     Provides an easy way to check if this node
        ///     is in the closed list.
        /// </summary>
        public bool InClosedList;

        /// <summary>
        ///     Provides an easy way to check if this node
        ///     is in the open list.
        /// </summary>
        public bool InOpenList;

        /// <summary>
        ///     This contains references to the for nodes surrounding
        ///     this tile (Up, Down, Left, Right).
        /// </summary>
        public SearchNode[] Neighbors;

        /// <summary>
        ///     A reference to the node that transferred this node to
        ///     the open list. This will be used to trace our path back
        ///     from the goal node to the start node.
        /// </summary>
        public SearchNode Parent;

        /// <summary>
        ///     Location on the map
        /// </summary>
        public Point Position;

        /// <summary>
        ///     If true, this tile can be walked on.
        /// </summary>
        public bool Walkable;
    }

    /// <summary>
    ///     An basic implementation of the A* algorithm.
    ///     Adopted from: https://xnatd.blogspot.com/2011/06/pathfinding-tutorial-part-1.html
    /// </summary>
    public class Pathfinder
    {
        // Holds the nodes that have already been searched.
        private readonly List<SearchNode> _closedList = new List<SearchNode>();

        // The height of the map.
        private readonly int _levelHeight;

        // The width of the map.
        private readonly int _levelWidth;

        // Holds search nodes that are available to search.
        private readonly List<SearchNode> _openList = new List<SearchNode>();

        // Stores an array of the walkable search nodes.
        private SearchNode[,] _searchNodes;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Pathfinder(TiledMap map)
        {
            _levelWidth = map.Width;
            _levelHeight = map.Height;

            InitializeSearchNodes(map);
        }

        /// <summary>
        ///     Returns an estimate of the distance between two points. (H)
        /// </summary>
        private static float Heuristic(Point point1, Point point2)
        {
            var (x1, y1) = point1;
            var (x2, y2) = point2;

            return Math.Abs(x1 - x2) +
                   Math.Abs(y1 - y2);
        }

        /// <summary>
        ///     Splits the level up into a grid of nodes.
        /// </summary>
        /// <param name="map"></param>
        private void InitializeSearchNodes(TiledMap map)
        {
            _searchNodes = new SearchNode[_levelWidth, _levelHeight];

            //For each of the tiles in our map, we
            // will create a search node for it.
            for (var x = 0; x < _levelWidth; x++)
            for (var y = 0; y < _levelHeight; y++)
            {
                //Create a search node to represent this tile.
                var node = new SearchNode
                {
                    Position = new Point(x, y), Walkable = map.GetTile("player", x, y).IsBlank
                    //Position = new Point(x, y), Walkable = !Globals.MapSystem.IsTileCollision(x, y)
                };

                // We only want to store nodes
                // that can be walked on.
                if (node.Walkable != true) continue;

                node.Neighbors = new SearchNode[4];
                _searchNodes[x, y] = node;
            }

            // Now for each of the search nodes, we will
            // connect it to each of its neighbours.
            for (var x = 0; x < _levelWidth; x++)
            for (var y = 0; y < _levelHeight; y++)
            {
                var node = _searchNodes[x, y];

                // We only want to look at the nodes that 
                // our enemies can walk on.
                if (node == null || node.Walkable == false) continue;

                // An array of all of the possible neighbors this 
                // node could have. (We will ignore diagonals for now.)
                var neighbors = new[]
                {
                    new Point(x, y - 1), // The node above the current node
                    new Point(x, y + 1), // The node below the current node.
                    new Point(x - 1, y), // The node left of the current node.
                    new Point(x + 1, y) // The node right of the current node
                };

                // We loop through each of the possible neighbors
                for (var i = 0; i < neighbors.Length; i++)
                {
                    var position = neighbors[i];

                    // We need to make sure this neighbour is part of the level.
                    if (position.X < 0 || position.X > _levelWidth - 1 ||
                        position.Y < 0 || position.Y > _levelHeight - 1)
                        continue;

                    var neighbor = _searchNodes[position.X, position.Y];

                    // We will only bother keeping a reference 
                    // to the nodes that can be walked on.
                    if (neighbor == null || neighbor.Walkable == false) continue;

                    // Store a reference to the neighbor.
                    node.Neighbors[i] = neighbor;
                }
            }
        }

        /// <summary>
        ///     Resets the state of the search nodes.
        /// </summary>
        private void ResetSearchNodes()
        {
            _openList.Clear();
            _closedList.Clear();

            for (var x = 0; x < _levelWidth; x++)
            for (var y = 0; y < _levelHeight; y++)
            {
                var node = _searchNodes[x, y];

                if (node == null) continue;

                node.InOpenList = false;
                node.InClosedList = false;

                node.DistanceTraveled = float.MaxValue;
                node.DistanceToGoal = float.MaxValue;
            }
        }

        /// <summary>
        ///     Use the parent field of the search nodes to trace
        ///     a path from the end node to the start node.
        /// </summary>
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            _closedList.Add(endNode);

            var parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path.
            while (parentTile != startNode)
            {
                _closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            var finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (var i = _closedList.Count - 1; i >= 0; i--)
                finalPath.Add(new Vector2(_closedList[i].Position.X * 32,
                    _closedList[i].Position.Y * 32));

            return finalPath;
        }

        /// <summary>
        ///     Returns the node with the smallest distance to goal.
        /// </summary>
        private SearchNode FindBestNode()
        {
            var currentTile = _openList[0];

            var smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            foreach (var node in _openList.Where(node => node.DistanceToGoal < smallestDistanceToGoal))
            {
                currentTile = node;
                smallestDistanceToGoal = currentTile.DistanceToGoal;
            }

            return currentTile;
        }

        /// <summary>
        ///     Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            // Only try to find a path if the start and end points are different.
            if (startPoint == endPoint) return new List<Vector2>();

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            var startNode = _searchNodes[startPoint.X, startPoint.Y];
            var endNode   = _searchNodes[endPoint.X, endPoint.Y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToGoal = Heuristic(startPoint, endPoint);
            startNode.DistanceTraveled = 0;

            _openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Step 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (_openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                var currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null) break;

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                foreach (var neighbor in currentNode.Neighbors)
                {
                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.Walkable == false) continue;

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    var distanceTraveled = currentNode.DistanceTraveled + 1;

                    // An estimate of the distance from this node to the end node.
                    var heuristic = Heuristic(neighbor.Position, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.InOpenList == false && neighbor.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value we just calculated.
                        neighbor.DistanceTraveled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + the estimated 
                        //     distance between the neighbouring node and goal node.
                        neighbor.DistanceToGoal = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active Node.
                        neighbor.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.InOpenList = true;
                        _openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.InOpenList || neighbor.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.DistanceTraveled > distanceTraveled)
                        {
                            neighbor.DistanceTraveled = distanceTraveled;
                            neighbor.DistanceToGoal = distanceTraveled + heuristic;

                            neighbor.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                _openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }
    }
}