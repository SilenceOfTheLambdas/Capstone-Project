using System;
using GrimGame.Character;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace GrimGame.Engine
{
    public class IsometricCamera
    {
        public readonly OrthographicCamera OrthographicCamera;
        private readonly Game1 _game1;
        
        public IsometricCamera(ViewportAdapter viewportAdapter, Game1 game1)
        {
            OrthographicCamera = new OrthographicCamera(viewportAdapter);
            _game1 = game1;
        }

        public void Follow(Player player)
        {
            OrthographicCamera.LookAt(TileToScreenCoords(player.Position));
        }

        public Vector2 TileToScreenCoords(Vector2 screen)
        {
            var (x, y) = screen;
            int tileWidth = _game1._map.TileWidth;
            int tileHeight = _game1._map.TileHeight;
            var originX = _game1._map.Height * tileWidth / 2;
            var tileY = y / tileHeight;
            var tileX = x / tileHeight;
            
            return new Vector2((tileX - tileY) * (tileWidth / 2) - 145, (tileX + tileY) * (tileHeight / 2) - 145);
        }

    }
}