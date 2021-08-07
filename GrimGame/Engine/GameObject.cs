#region Imports

using System;
using GrimGame.Game;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;
using MLEM.Extended.Tiled;

#endregion

namespace GrimGame.Engine
{
    /// <summary>
    ///     Represents all of the visible objects in the game.
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        ///     Creates a new game object.
        /// </summary>
        /// <param name="x">X world position.</param>
        /// <param name="y">Y world position.</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        protected GameObject(int x = 0, int y = 0, int w = 0, int h = 0)
        {
            // _____ Dimensions _____ //
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Bounds = new Rectangle(x, y, Width, Height);

            // _____ Properties _____ //
            Active = true;
            Visible = true;
            Collision = false;
        }

        #region Properties

        /// <summary>
        ///     Is this game actively being updated?
        /// </summary>
        public bool Active;

        /// <summary>
        ///     Is this game object being drawn?
        /// </summary>
        public bool Visible;

        /// <summary>
        ///     The position of this game object.
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        ///     The position of this game object in 'Tile Space'.
        /// </summary>
        public Vector2 TilePosition =>
            new Vector2(Globals.MapSystem.Map.GetTile("__player__", (int) X, (int) Y).X,
                Globals.MapSystem.Map.GetTile("__player__", (int) X, (int) Y).Y);

        /// <summary>
        ///     The size of this object.
        /// </summary>
        protected Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = (int) value.X;
                Height = (int) value.Y;
            }
        }

        /// <summary>
        ///     Represents the rotation of this game object.
        /// </summary>
        protected float Rotation { get; set; }

        /// <summary>
        ///     The bounding box for this game object.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        ///     The origin point of this game object.
        /// </summary>
        protected Vector2 Origin;

        /// <summary>
        ///     The velocity at which this objects moves.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        ///     Is this object colliding with something?
        /// </summary>
        public bool Collision;

        /// <summary>
        ///     Is the object enabled?
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        ///     The height of this game object.
        /// </summary>
        public int Height;

        // _____ Properties _____ //

        /// <summary>
        ///     The speed this object moves.
        /// </summary>
        public float Speed;

        /// <summary>
        ///     The width of this game object.
        /// </summary>
        public int Width;

        // _____ Dimensions _____ //

        /// <summary>
        ///     The X and Y position of this game object.
        /// </summary>
        public float X, Y;

        // _____ Sprite _____ //

        /// <summary>
        ///     The sprite texture of this object
        /// </summary>
        protected Sprite Sprite;

        protected Vector2 Scale;

        public BoxCollider BoxCollider;

        #endregion

        #region Methods

        /// <summary>
        ///     Anything is this function will be ran every frame.
        /// </summary>
        /// <param name="gameTime">The amount of time that has passed since the last frame.</param>
        public virtual void Update(GameTime gameTime)
        {
            CollisionDetection();
            var objectsList = SceneManager.GetActiveScene.ObjectManager.Objects;
            foreach (var gameObject in objectsList)
            {
                if (!(gameObject is Player) && IsColliding(ref gameObject.BoxCollider.Bounds))
                {
                    OnCollisionEnter(gameObject);
                    Collision = true;
                    break;
                }

                if (Collision && !IsColliding(ref gameObject.BoxCollider.Bounds))
                {
                    OnCollisionExit();
                    Collision = false;
                    break;
                }
            }
        }

        // _____ Setters _____ //

        /// <summary>
        ///     Set the bounds of the object
        /// </summary>
        /// <param name="x">The x position of the bounding box</param>
        /// <param name="y">The y position of the bounding box</param>
        /// <param name="w">The width of the bounding box</param>
        /// <param name="h">The height of the bounding box</param>
        public void SetBounds(float x, float y, int w, int h)
        {
            Bounds = new Rectangle((int) x, (int) y, w, h);
        }

        // _____ Getters _____ //

        /// <summary>
        ///     Destroy a given game object, removing it from the list of objects
        ///     stored in <see cref="ObjectManager" />.
        /// </summary>
        /// <param name="gameObject">The game object to destroy</param>
        protected static void Destroy(GameObject gameObject)
        {
            SceneManager.GetActiveScene.ObjectManager.Remove(gameObject);
        }

        /// <summary>
        ///     Gets the distance from one point to another.
        /// </summary>
        /// <param name="pos">The start position</param>
        /// <param name="target">The end position</param>
        /// <returns>The shortest distance</returns>
        public static float GetDistance(Vector2 pos, Vector2 target)
        {
            return (float) Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
        }

        /// <summary>
        ///     Moves an object 'smoothly'.
        ///     Work taken from: https://www.youtube.com/watch?v=yYNrmsmEcy8
        /// </summary>
        /// <param name="focus">The point to move towards</param>
        /// <param name="pos">The current position of this object</param>
        /// <param name="speed">The speed at which to move at</param>
        /// <returns>Direction to move towards</returns>
        protected static Vector2 RadialMovement(Vector2 focus, Vector2 pos, float speed)
        {
            var dist = GetDistance(pos, focus);

            if (dist <= speed)
                return focus - pos;
            return (focus - pos) * speed / dist;
        }

        private bool IsColliding(ref Rectangle other)
        {
            var collision = BoxCollider.Bounds.Intersects(other);
            return collision;
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        ///     Draws this object onto the screen.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        ///     Detects collisions between the player and any collision objects.
        /// </summary>
        private void CollisionDetection()
        {
            foreach (var collisionObject in MapSystem.CollisionObjects)
            {
                if (Velocity.X > 0 && IsTouchingLeft(collisionObject) ||
                    Velocity.X < 0 && IsTouchingRight(collisionObject))
                    Velocity.X = 0;


                if (Velocity.Y > 0 && IsTouchingTop(collisionObject) ||
                    Velocity.Y < 0 && IsTouchingBottom(collisionObject))
                    Velocity.Y = 0;
            }
        }

        /// <summary>
        ///     If an object has collided with this box collider.
        /// </summary>
        /// <param name="other">The object that has collided with *this* object</param>
        public virtual void OnCollisionEnter(GameObject other)
        {
        }

        /// <summary>
        ///     When an object exits a collision
        /// </summary>
        public virtual void OnCollisionExit()
        {
        }

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
            return boxCollider.Right + Velocity.X > collisionRectangle.Left &&
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
            return boxCollider.Left + Velocity.X < collisionRectangle.Right &&
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
            return boxCollider.Bottom + Velocity.Y > collisionRectangle.Top &&
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
            return boxCollider.Top + Velocity.Y < collisionRectangle.Bottom &&
                   boxCollider.Bottom > collisionRectangle.Bottom &&
                   boxCollider.Right > collisionRectangle.Left &&
                   boxCollider.Left < collisionRectangle.Right;
        }

        #endregion
    }
}