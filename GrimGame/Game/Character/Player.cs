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
        public enum PlayerMovementStates
        {
            Walking,
            Running,
            FrozenXPos,
            FrozenXNeg,
            FrozenYPos,
            FrozenYNeg,
            Idle
        }
        public PlayerMovementStates PlayerMovementState = PlayerMovementStates.Idle;
        
        // _____ Transform _____ //
        /// <summary>
        /// The player's tile position.
        /// </summary>
        public Vector2 TilePosition;
        
        /// <summary>
        /// The origin point of the player's sprite.
        /// </summary>
        private new Vector2 Origin { get; set; }

        // _____ Properties _____ //
        public BoxCollider BoxCollider;
        
        private readonly MapSystem _mapSystem;
        private readonly OrthographicCamera _camera;

        public Player(MapSystem mapSystem, OrthographicCamera camera,Texture2D playerSprite)
        {
            _mapSystem = mapSystem;
            _camera = camera;
            Sprite = playerSprite;
        }
        
        public override void Init(MainGame g)
        {
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
                new Vector2(Position.X - ((Sprite.Width / 2) / 2), Position.Y),
                new Point(Sprite.Width / 2,  16));
        }

        public override void Update(MainGame g)
        {
            var x = (ushort) (Position.X / 32);
            var y = (ushort) (Position.Y / 32);
            
            TilePosition = new Vector2(x, y);

            // _____ Update Box Collider _____ //
            BoxCollider.Update(g);
            
            // _____ Update Player Position _____ //
            Move();

            CollisionDetection();
        }

        private void CollisionDetection()
        {
            foreach (var rectangle in _mapSystem.CollisionObjects)
            {
                if (BoxCollider.Bounds.Intersects(rectangle.Key))
                {
                    PlayerMovementState = PlayerMovementStates.FrozenYPos;
                }

                if (BoxCollider.Bounds.Intersects(rectangle.Key) && Position.Y >= rectangle.Key.Bottom)
                {
                    PlayerMovementState = PlayerMovementStates.FrozenYNeg;
                }

                if (BoxCollider.Bounds.Intersects(rectangle.Key) && Position.X <= rectangle.Key.Left)
                {
                    PlayerMovementState = PlayerMovementStates.FrozenXPos;
                }

                if (BoxCollider.Bounds.Intersects(rectangle.Key) && Position.X >= rectangle.Key.Right)
                {
                    PlayerMovementState = PlayerMovementStates.FrozenXNeg;
                }
                
//                if (BoxCollider.Bounds.Intersects(rectangle) && Position.X <= rectangle.Left && Position.)
            }
        }

        public override void Draw(MainGame g)
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            // Drawing of player sprite
            Globals.SpriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Origin, 
                new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.1f);
            Globals.SpriteBatch.End();
        }

        /// <summary>
        /// Moves the player.
        /// </summary>
        private void Move()
        {
            _camera.LookAt(Position);
            
            // Update the BoxCollider bounding box position
            BoxCollider.UpdatePosition(new Point((int) (Position.X - ((Sprite.Width / 2) / 2)), 
                (int) (Position.Y - 16)));
            
            if (PlayerMovementState != PlayerMovementStates.FrozenYNeg)
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    PlayerMovementState = PlayerMovementStates.Walking;
                    Position += new Vector2(0, -1);
                }

            if (PlayerMovementState != PlayerMovementStates.FrozenYPos)
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    PlayerMovementState = PlayerMovementStates.Walking;
                    Position += new Vector2(0, 1);
                }

            if (PlayerMovementState != PlayerMovementStates.FrozenXNeg)
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    PlayerMovementState = PlayerMovementStates.Walking;
                    Position += new Vector2(-1, 0);
                }

            if (PlayerMovementState != PlayerMovementStates.FrozenXPos)
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    PlayerMovementState = PlayerMovementStates.Walking;
                    Position += new Vector2(1, 0);
                }
        }
    }
}