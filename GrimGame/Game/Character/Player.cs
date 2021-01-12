#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MapSystem        _mapSystem;
        private          AnimationManager _animationManager;
        private          float            _defaultWalkSpeed;

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
            Scale = new Vector2(1.2f, 1.2f);

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
                Width = 19,
                Height = 29
            };
            Texture = Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_up");
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height);
            _defaultWalkSpeed = Speed;
            Height = (int) (Sprite.Height * PlayerScale);
            Width = (int) (Sprite.Width * PlayerScale);

            foreach (var objectLayer in _mapSystem.Map.ObjectLayers)
            foreach (var layerObject in objectLayer.Objects)
                if (layerObject.Name.ToLower().Equals("playerspawn"))
                    Position = layerObject.Position;


            _animationManager = new AnimationManager(Sprite.Animations.FirstOrDefault().Value);

            BoxCollider = new BoxCollider(new Vector2(Position.X, Position.Y),
                new Point(19, 16));
        }

        public override void Update(GameTime gameTime)
        {
            BoxCollider.Update(gameTime);

            _animationManager.Position = Position;
            _animationManager.Origin = Origin;
            _animationManager.Scale = Scale;

            _animationManager.Update(gameTime);

            var x = (ushort) (Position.X / 32);
            var y = (ushort) (Position.Y / 32);

            TilePosition = new Vector2(x, y);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Running;
            else if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Walking;

            Move();

            UpdatePlayerAnimationDirections();

            _camera.LookAt(Position);

            base.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: SamplerState.PointClamp);

            if (_animationManager != null)
                _animationManager.Draw();
            else throw new Exception("Sprite has no animation of texture assigned!");

            Globals.SpriteBatch.End();
        }

        private void Move()
        {
            Speed = _playerMovementState switch
            {
                PlayerMovementStates.Walking => _defaultWalkSpeed,
                PlayerMovementStates.Running => RunningSpeed,
                PlayerMovementStates.Idle => 0f,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Velocity.Y = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Velocity.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Velocity.X = Speed;

            // _____ Detecting Collisions _____ //
            BoxCollider.UpdatePosition(new Point((int) (Position.X - Width / 2),
                (int) (Position.Y - 16)));
        }

        private void UpdatePlayerAnimationDirections()
        {
            if (Velocity.X > 0)
                _animationManager.Play(Sprite.Animations["walk_right"]);
            else if (Velocity.X < 0)
                _animationManager.Play(Sprite.Animations["walk_left"]);
            else if (Velocity.Y > 0)
                _animationManager.Play(Sprite.Animations["walk_down"]);
            else if (Velocity.Y < 0)
                _animationManager.Play(Sprite.Animations["walk_up"]);
            else _animationManager.Stop();
        }

        private enum PlayerMovementStates
        {
            Walking,
            Running,
            Idle
        }
    }
}