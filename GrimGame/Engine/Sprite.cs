using System;
using System.Collections.Generic;
using System.Linq;
using GrimGame.Engine.Models;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GrimGame.Engine
{
    public sealed class Sprite
    {
        public Sprite(Dictionary<string, Animation> animations)
        {
            _velocity = new Vector2();
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);

            BoxCollider = new BoxCollider(new Vector2(Position.X, Position.Y),
                new Point(19, 16));
        }

        public void Draw()
        {
            if (_texture != null)
                Globals.SpriteBatch.Draw(_texture, Position, null, Color.White, 0f, Origin,
                    Scale, SpriteEffects.None, 0.1f);
            else if (_animationManager != null)
                _animationManager.Draw();
            else throw new Exception("Sprite has no animation of texture assigned!");
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                _velocity.Y = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                _velocity.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                _velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                _velocity.X = Speed;

            // _____ Detecting Collisions _____ //
            BoxCollider.UpdatePosition(new Point((int) (Position.X - Width / 2),
                (int) (Position.Y - 16)));
        }

        private void SetAnimations()
        {
            if (_velocity.X > 0)
                _animationManager.Play(_animations["walk_right"]);
            else if (_velocity.X < 0)
                _animationManager.Play(_animations["walk_left"]);
            else if (_velocity.Y > 0)
                _animationManager.Play(_animations["walk_down"]);
            else if (_velocity.Y < 0)
                _animationManager.Play(_animations["walk_up"]);
            else _animationManager.Stop();
        }

        public void Update(GameTime gameTime, Scene scene)
        {
            // _____ Update Box Collider _____ //
            BoxCollider.Update(scene, gameTime);

            Move();

            SetAnimations();

            _animationManager.Update(gameTime);

            CollisionDetection();

            Position += _velocity;
            _velocity = Vector2.Zero;
        }

        /// <summary>
        ///     Detects collisions between the player and any collision objects.
        /// </summary>
        private void CollisionDetection()
        {
            foreach (var collisionObject in MapSystem.CollisionObjects)
            {
                if (_velocity.X > 0 && IsTouchingLeft(collisionObject) ||
                    _velocity.X < 0 && IsTouchingRight(collisionObject))
                    _velocity.X = 0;


                if (_velocity.Y > 0 && IsTouchingTop(collisionObject) ||
                    _velocity.Y < 0 && IsTouchingBottom(collisionObject))
                    _velocity.Y = 0;
            }
        }

        #region Fields

        private readonly AnimationManager              _animationManager;
        private readonly Dictionary<string, Animation> _animations;
        private          Vector2                       _position;
        private          Vector2                       _origin;
        private          Vector2                       _scale;
        private readonly Texture2D                     _texture;

        #endregion

        #region Properties

        public int Width { get; set; }
        public int Height { get; set; }

        public BoxCollider BoxCollider { get; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public Vector2 Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                if (_animationManager != null)
                    _animationManager.Origin = _origin;
            }
        }

        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                if (_animationManager != null)
                    _animationManager.Scale = _scale;
            }
        }

        public  float   Speed = 1f;
        private Vector2 _velocity;

        #endregion

        #region CollisionDetection

        /// <summary>
        ///     Is the player touching the left of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingLeft(Rectangle collisionRectangle)
        {
            var boxCollider = BoxCollider.Bounds;
            return boxCollider.Right + _velocity.X > collisionRectangle.Left &&
                   boxCollider.Left < collisionRectangle.Left &&
                   boxCollider.Bottom > collisionRectangle.Top &&
                   boxCollider.Top < collisionRectangle.Bottom;
        }

        /// <summary>
        ///     Is the player touching the right of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingRight(Rectangle collisionRectangle)
        {
            var boxCollider = BoxCollider.Bounds;
            return boxCollider.Left + _velocity.X < collisionRectangle.Right &&
                   boxCollider.Right > collisionRectangle.Right &&
                   boxCollider.Bottom > collisionRectangle.Top &&
                   boxCollider.Top < collisionRectangle.Bottom;
        }

        /// <summary>
        ///     Is the player touching the Top of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingTop(Rectangle collisionRectangle)
        {
            var boxCollider = BoxCollider.Bounds;
            return boxCollider.Bottom + _velocity.Y > collisionRectangle.Top &&
                   boxCollider.Top < collisionRectangle.Top &&
                   boxCollider.Right > collisionRectangle.Left &&
                   boxCollider.Left < collisionRectangle.Right;
        }

        /// <summary>
        ///     Is the player touching the bottom of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingBottom(Rectangle collisionRectangle)
        {
            var boxCollider = BoxCollider.Bounds;
            return boxCollider.Top + _velocity.Y < collisionRectangle.Bottom &&
                   boxCollider.Bottom > collisionRectangle.Bottom &&
                   boxCollider.Right > collisionRectangle.Left &&
                   boxCollider.Left < collisionRectangle.Right;
        }

        #endregion
    }
}