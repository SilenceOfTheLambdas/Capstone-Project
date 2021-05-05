#region Imports

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public sealed class Player : GameObject
    {
        /// <summary>
        ///     The draw scale of the player.
        /// </summary>
        private const float PlayerScale = 1.5f;

        // _____ Attack _____ //
        private const    int                AttackDamage = 40; // How much HP the player deals when attacking
        private readonly OrthographicCamera _camera;

        // _____ References _____ //
        private readonly MapSystem        _mapSystem;
        private          AnimationManager _animationManager;
        private          int              _currentHp;
        private          float            _defaultWalkSpeed;
        private          bool             _enemyInAttackRange;
        private          Enemy            _enemyToHit;

        private PlayerDirection _playerDirection;

        private PlayerMovementStates _playerMovementState = PlayerMovementStates.Idle;
        private float                _timerForAttacks; // only used to count the number of seconds
        public  float                AttackTimer = 1.2f; // How often the player can attack (in seconds)
        private int                  i           = 0;

        // _____ Properties _____ //
        public float RunningSpeed;

        /// <summary>
        ///     Creates a new player.
        /// </summary>
        /// <param name="mapSystem">The map the player will be draw to</param>
        /// <param name="camera">A camera that will follow the player</param>
        public Player(MapSystem mapSystem, OrthographicCamera camera)
        {
            _mapSystem = mapSystem;
            _camera = camera;
            Score = 0;
            SceneManager.GetActiveScene.ObjectManager.Add(this);
        }

        /// <summary>
        ///     The total score the player accumulated.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        ///     The currency for buying upgrades, directly relates to <see cref="Score" />
        /// </summary>
        public int Coins { get; set; }

        // ____ Health ____ //
        /// <summary>
        ///     The maximum number of health points.
        /// </summary>
        public int MaxHp { get; set; }

        /// <summary>
        ///     The current number of health points.
        /// </summary>
        public int CurrentHp
        {
            get => _currentHp;
            set => _currentHp = Math.Clamp(value, 0, MaxHp);
        }

        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        public void Init()
        {
            Scale = new Vector2(1.2f, 1.2f);

            Sprite = new Sprite(new Dictionary<string, Animation>
            {
                {
                    "walk_up",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_up"), 6)
                },
                {
                    "walk_down",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_down"), 6)
                },
                {
                    "walk_left",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_left"), 6)
                },
                {
                    "walk_right",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_right"), 6)
                },
                {
                    "attack_left",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/attack_left"), 3)
                },
                {
                    "attack_right",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/attack_right"), 3)
                },
                {
                    "attack_up",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/attack_up"), 3)
                },
                {
                    "attack_down",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/attack_down"), 3)
                }
            })
            {
                Width = 14,
                Height = 33
            };
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

            Coins = Score;
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Position = Position;
            _animationManager.Origin = Origin;
            _animationManager.Scale = Scale;

            _animationManager.Update(gameTime);

            _playerMovementState = Keyboard.GetState().IsKeyDown(Keys.LeftShift)
                ? PlayerMovementStates.Running
                : PlayerMovementStates.Walking;

            Move();
            UpdatePlayerAnimationDirections();

            _camera.LookAt(Position);

            _timerForAttacks += gameTime.GetElapsedSeconds();

            if (_timerForAttacks >= AttackTimer)
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _animationManager.Play(Sprite.Animations[$"attack_{_playerDirection.ToString().ToLower()}"]);
                    Attack();
                    _timerForAttacks -= AttackTimer;
                }

            base.Update(gameTime);

            // MUST be placed after base.update()
            Position += Velocity;
            Velocity = Vector2.Zero;

            if (_currentHp <= 0)
                Kill();
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: new SamplerState {Filter = TextureFilter.Point});

            if (_animationManager != null)
                _animationManager.Draw();
            else throw new Exception("Sprite has no animation of texture assigned!");

            Globals.SpriteBatch.End();
        }

        /// <summary>
        ///     Moves the player when the player presses the movement keys.
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
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

        /// <summary>
        ///     Updates the player's <see cref="_playerDirection" /> and animation based on their direction.
        /// </summary>
        private void UpdatePlayerAnimationDirections()
        {
            if (Velocity.X > 0)
            {
                _animationManager.Play(Sprite.Animations["walk_right"]);
                _playerDirection = PlayerDirection.Right;
            }
            else if (Velocity.X < 0)
            {
                _animationManager.Play(Sprite.Animations["walk_left"]);
                _playerDirection = PlayerDirection.Left;
            }
            else if (Velocity.Y > 0)
            {
                _animationManager.Play(Sprite.Animations["walk_down"]);
                _playerDirection = PlayerDirection.Down;
            }
            else if (Velocity.Y < 0)
            {
                _animationManager.Play(Sprite.Animations["walk_up"]);
                _playerDirection = PlayerDirection.Up;
            }
            else
            {
                _animationManager.Stop();
            }
        }

        /// <summary>
        ///     Inflict damage upon a target
        /// </summary>
        private void Attack()
        {
            if (_enemyInAttackRange)
            {
                // If the enemy is within attack range, attack
                _enemyToHit.CurrentHp -= AttackDamage;
                if (_enemyToHit.CurrentHp <= 0)
                {
                    // Update player's score
                    Score += 1;
                    Coins += 1;
                }
            }
        }

        public override void OnCollisionEnter(GameObject other)
        {
            // Check to see if an enemy collided with us
            if (other is Enemy enemy && other.Active && other.Enabled)
            {
                _enemyToHit = enemy;
                _enemyInAttackRange = true;
            }
        }

        public override void OnCollisionExit()
        {
            _enemyInAttackRange = false;
        }

        /// <summary>
        ///     Kills the player, and ends the game.
        /// </summary>
        private void Kill()
        {
            UiManager.DisplayEndScreen();
            SceneManager.GetActiveScene.ObjectManager.Clear();
        }

        /// <summary>
        ///     Directions the player could be facing.
        /// </summary>
        private enum PlayerDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        /// <summary>
        ///     A bunch of states representing the player's movement.
        /// </summary>
        private enum PlayerMovementStates
        {
            /// <summary>
            ///     Walking
            /// </summary>
            Walking,

            /// <summary>
            ///     Running at <see cref="Player.RunningSpeed" />
            /// </summary>
            Running,

            /// <summary>
            ///     Player is not moving.
            /// </summary>
            Idle
        }
    }
}