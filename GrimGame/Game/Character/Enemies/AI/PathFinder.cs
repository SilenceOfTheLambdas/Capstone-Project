using System;
using System.Collections.Generic;
using System.Linq;
using GrimGame.Engine;
using GrimGame.Game;
using MLEM.Extended.Tiled;
using MonoGame.Extended.Tiled;

namespace GrimGame.Character.Enemies.AI
{
   public class PathFinder
   {
      public event EventHandler Updated;

      private void OnUpdated()
      {
         Updated?.Invoke(null, EventArgs.Empty);
      }

      public MapSystem MapSystem { get; set; }
      public TiledMap  Map { get; set; }
      public Node Start { get; set; }
      public Node End { get; set; }
      public int NodeVisits { get; set; }
      public double ShortestPathLength { get; set; }
      public double ShortestPathCost { get; private set; }

      public PathFinder(MapSystem mapSystem)
      {
         MapSystem = mapSystem;
         Map = mapSystem.Map;
         
      }
   }
}