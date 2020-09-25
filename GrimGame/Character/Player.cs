using System;
using System.Collections.Generic;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled;

namespace GrimGame.Character
{
    public class Player
    {
        public Vector2 Position;

        private Texture2D _playerSprite;
        public Texture2D PlayerSprite
        {
            get => _playerSprite;
            set => _playerSprite = value;
        }

        private Game1 _game1;

        public Player(Game1 game, Texture2D playerSprite)
        {
            _game1 = game;
            PlayerSprite = playerSprite;
        }

        public void SpawnPlayer()
        {
            foreach (var objectLayer in _game1._map.ObjectLayers)
            {
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (layerObject.Name.ToLower().Equals("playerstart"))
                    {
                        Position = layerObject.Position;
                    }
                }
            }

            _game1._spriteBatch.Draw(_playerSprite, _game1._camera.TileToScreenCoords(Position), null, Color.White, 0f, Vector2.Zero,
                new Vector2(1f, 1f), SpriteEffects.None, 0);
        }

        public void Update()
        {
            _game1._camera.Follow(this);
            Move();
        }

        public void Move()
        {
            _game1._spriteBatch.Draw(_playerSprite, _game1._camera.TileToScreenCoords(Position), null, Color.White, 0f, Vector2.Zero,
                new Vector2(1f, 1f), SpriteEffects.None, 0);
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position += new Vector2(-1, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                
            }
        }
    }
}