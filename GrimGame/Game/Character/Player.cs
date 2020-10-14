#region Imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
#endregion

namespace GrimGame.Game.Character
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

        private MapSystem _mapSystem;
        private OrthographicCamera _camera;

        public Player(MapSystem mapSystem, OrthographicCamera camera,Texture2D playerSprite)
        {
            _mapSystem = mapSystem;
            _camera = camera;
            PlayerSprite = playerSprite;
            
            foreach (var objectLayer in mapSystem.Map.ObjectLayers)
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
            _camera.LookAt(Position);
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