using System;
using System.Collections.Generic;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
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
            
            foreach (var objectLayer in _game1._map.ObjectLayers)
            {
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (layerObject.Name.ToLower().Equals("playerspawn"))
                    {
                        Position = layerObject.Position;
                    }
                }
            }
        }

        public void Update()
        {
            Move();
        }

        public void Move()
        {
            _game1._camera.LookAt(Position);
            _game1._spriteBatch.Draw(_playerSprite, Position, null, Color.White, 0f, Vector2.Zero,
                new Vector2(0.25f, 0.25f), SpriteEffects.None, 1);
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position += new Vector2(1, 0);
            }
        }
    }
}