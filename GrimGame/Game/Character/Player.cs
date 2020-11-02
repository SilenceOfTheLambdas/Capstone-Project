#region Imports
using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
#endregion

namespace GrimGame.Game.Character
{
    /// <summary>
    /// The playable character.
    /// </summary>
    public class Player : GameObject
    {
        private enum PlayerMovementStates
        {
            Walking,
            Running = 12,
            Idle
        }
        private PlayerMovementStates _playerMovementState = PlayerMovementStates.Idle;

        private enum Direction
        {
            Up,
            Left,
            Right,
            Down
        }
        private Direction _playerDirection = Direction.Down;
        
        // _____ Transform _____ //
        /// <summary>
        /// The player's tile position.
        /// </summary>
        public Vector2 TilePosition;
        
        /// <summary>
        /// The origin point of the player's sprite.
        /// </summary>
        //private new Vector2 Origin { get; set; }

        // _____ Properties _____ //
        public BoxCollider BoxCollider;
        
        // _____ References _____ //
        private readonly MapSystem _mapSystem;
        private readonly OrthographicCamera _camera;

        public Player(MapSystem mapSystem, OrthographicCamera camera)
        {
            _mapSystem = mapSystem;
            _camera = camera;
        }
        
        public override void Init(MainGame g)
        {
            base.Sprite = Globals.ContentManager.Load<Texture2D>("Sprites/Player/down_walk1");
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height);

            foreach (var objectLayer in _mapSystem.Map.ObjectLayers)
            {
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (layerObject.Name.ToLower().Equals("playerspawn"))
                    {
                        Position = layerObject.Position;
                    }
                }
            }
            BoxCollider = new BoxCollider(this, 
                new Vector2(Position.X, Position.Y),
                new Point(Sprite.Width,  16));
        }

        public override void Update(MainGame g)
        {
            var x = (ushort) (Position.X / 32);
            var y = (ushort) (Position.Y / 32);
            
            TilePosition = new Vector2(x, y);

            // _____ Update Box Collider _____ //
            BoxCollider.Update(g);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Running;
            if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
                _playerMovementState = PlayerMovementStates.Walking;
            
            // _____ Update Player Position _____ //
            Move();
            
            // _____ Player Direction _____ //
            UpdatePlayerDirection();

            // _____ Detecting Collisions _____ //
            CollisionDetection();

            // _____ Update Player Position Based on velocity _____ //
            Position += Velocity;
            
            // set to zero to stop moving when user stops pressing movement keys
            Velocity = Vector2.Zero;
        }

        /// <summary>
        /// Detects collisions between the player and any collision objects.
        /// </summary>
        private void CollisionDetection()
        {
            foreach (var (collisionObject, _) in _mapSystem.CollisionObjects)
            {
                if (this.Velocity.X > 0 && IsTouchingLeft(collisionObject) ||
                    this.Velocity.X < 0 && IsTouchingRight(collisionObject))
                    this.Velocity.X = 0;

                    
                if (this.Velocity.Y > 0 && IsTouchingTop(collisionObject) ||
                    this.Velocity.Y < 0 && IsTouchingBottom(collisionObject))
                    this.Velocity.Y = 0;
            }
        }

        public override void Draw(MainGame g)
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            // Drawing of player sprite
            Globals.SpriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Origin, 
                new Vector2(1.5f, 1.5f), SpriteEffects.None, 0.1f);
            Globals.SpriteBatch.End();
        }

        /// <summary>
        /// Moves the player according to it's velocity.
        /// </summary>
        private void Move()
        {
            _camera.LookAt(Position);
            
            // Update the BoxCollider bounding box position
            BoxCollider.UpdatePosition(new Point((int) (Position.X - ((Sprite.Width / 2))), 
                (int) (Position.Y - 16)));

            if (_playerMovementState == PlayerMovementStates.Running)
                Speed = (float) PlayerMovementStates.Running;
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _playerMovementState = PlayerMovementStates.Walking;
                _playerDirection = Direction.Up;
                Velocity.Y = -Speed;
                
            } else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _playerMovementState = PlayerMovementStates.Walking;
                _playerDirection = Direction.Down;
                Velocity.Y = Speed;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _playerMovementState = PlayerMovementStates.Walking;
                _playerDirection = Direction.Left;
                Velocity.X = -Speed;
            } else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _playerMovementState = PlayerMovementStates.Walking;
                _playerDirection = Direction.Right;
                Velocity.X = Speed;
            }
        }

        /// <summary>
        /// Updates the player's sprite based on the direction they are facing.
        /// </summary>
        private void UpdatePlayerDirection()
        {
            Sprite = _playerDirection switch
            {
                Direction.Down => Globals.ContentManager.Load<Texture2D>("Sprites/Player/down_walk1"),
                Direction.Up => Globals.ContentManager.Load<Texture2D>("Sprites/Player/up_walk1"),
                Direction.Left => Globals.ContentManager.Load<Texture2D>("Sprites/Player/left_walk1"),
                Direction.Right => Globals.ContentManager.Load<Texture2D>("Sprites/Player/right_walk1"),
                _ => Globals.ContentManager.Load<Texture2D>("Sprites/Player/down_walk1")
            };
        }

        #region CollisionDetection

        /// <summary>
        /// Is the player touching the left of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingLeft(Rectangle collisionRectangle)
        {
            var playerBoxCollider = this.BoxCollider.Bounds;
            return playerBoxCollider.Right + this.Velocity.X > collisionRectangle.Left &&
                   playerBoxCollider.Left < collisionRectangle.Left &&
                   playerBoxCollider.Bottom > collisionRectangle.Top &&
                   playerBoxCollider.Top < collisionRectangle.Bottom;
        }

        /// <summary>
        /// Is the player touching the right of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingRight(Rectangle collisionRectangle)
        {
            var playerBoxCollider = this.BoxCollider.Bounds;
            return playerBoxCollider.Left + this.Velocity.X < collisionRectangle.Right &&
                   playerBoxCollider.Right > collisionRectangle.Right &&
                   playerBoxCollider.Bottom > collisionRectangle.Top &&
                   playerBoxCollider.Top < collisionRectangle.Bottom;
        }

        /// <summary>
        /// Is the player touching the Top of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingTop(Rectangle collisionRectangle)
        {
            var playerBoxCollider = this.BoxCollider.Bounds;
            return playerBoxCollider.Bottom + this.Velocity.Y > collisionRectangle.Top &&
                   playerBoxCollider.Top < collisionRectangle.Top &&
                   playerBoxCollider.Right > collisionRectangle.Left &&
                   playerBoxCollider.Left < collisionRectangle.Right;
        }

        /// <summary>
        /// Is the player touching the bottom of this collisionRectangle.
        /// </summary>
        /// <param name="collisionRectangle">The collisionRectangle to check against</param>
        /// <returns></returns>
        private bool IsTouchingBottom(Rectangle collisionRectangle)
        {
            var playerBoxCollider = this.BoxCollider.Bounds;
            return playerBoxCollider.Top + this.Velocity.Y < collisionRectangle.Bottom &&
                   playerBoxCollider.Bottom > collisionRectangle.Bottom &&
                   playerBoxCollider.Right > collisionRectangle.Left &&
                   playerBoxCollider.Left < collisionRectangle.Right;
        }
        #endregion
    }
}