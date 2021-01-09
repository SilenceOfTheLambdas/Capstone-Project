#region Imports

using System.Collections.Generic;
using GrimGame.Engine;
using GrimGame.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

#endregion

namespace GrimGame.Game.Character
{
    /// <summary>
    ///     The playable character.
    /// </summary>
    public class Player : GameObject
    {
        private const    float              PlayerScale = 1.5f;
        private readonly OrthographicCamera _camera;

        // _____ References _____ //
        private readonly MapSystem _mapSystem;
        private          float     _defaultWalkSpeed;

        private PlayerMovementStates _playerMovementState = PlayerMovementStates.Idle;

        // _____ Properties _____ //
        public float RunningSpeed;

        // _____ Transform _____ //
        /// <summary>
        ///     The player's tile position.
        /// </summary>
        public Vector2 TilePosition;

        public Player(MapSystem mapSystem, OrthographicCamera camera)
        {
            _mapSystem = mapSystem;
            _camera = camera;
        }

        public override void Init()
        {
            Sprite = new Sprite(new Dictionary<string, Animation>
            {
                {
                    "walk_up",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_up"), 2)
                },
                {
                    "walk_down",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_down"), 2)
                },
                {
                    "walk_left",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_left"), 2)
                },
                {
                    "walk_right",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_right"), 2)
                }
            })
            {
                Position = Position,
                Speed = Speed,
                Width = 19,
                Height = 29
            };
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height);
            _defaultWalkSpeed = Speed;
            Height = (int) (Sprite.Height * PlayerScale);
            Width = (int) (Sprite.Width * PlayerScale);

            foreach (var objectLayer in _mapSystem.Map.ObjectLayers)
            foreach (var layerObject in objectLayer.Objects)
                if (layerObject.Name.ToLower().Equals("playerspawn"))
                    Position = layerObject.Position;

            // Load Sprite Animations
            Sprite.Position = Position;
            Sprite.Origin = Origin;
            Sprite.Scale = new Vector2(PlayerScale, PlayerScale);
            Sprite.Speed = Speed;
        }

        public override void Update(Scene scene, GameTime gameTime)
        {
            // Update Player's Position variables
            Position = Sprite.Position;

            var x = (ushort) (Position.X / 32);
            var y = (ushort) (Position.Y / 32);

            TilePosition = new Vector2(x, y);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Running;
            else if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Walking;

            // ____ Update Sprite Animations ____ //
            Sprite.Update(gameTime, scene);
            _camera.LookAt(Sprite.Position);
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: SamplerState.PointClamp);
            // Drawing of player sprite
            Sprite.Draw();
            Globals.SpriteBatch.End();
        }

        private enum PlayerMovementStates
        {
            Walking,
            Running,
            Idle
        }

        private enum Direction
        {
            Up,
            Left,
            Right,
            Down
        }
    }
}