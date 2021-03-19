#region Imports

using System;
using GrimGame.Game;
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
        public Rectangle Bounds;

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

            SceneManager.GetActiveScene.ObjectManager.Objects.ForEach(o =>
            {
                if (o != this)
                    // Check to see if this object has a box collider, and that it is enabled
                    if (o.BoxCollider != null)
                        if (BoxCollider != null && o.BoxCollider.Bounds.Intersects(BoxCollider.Bounds))
                            OnCollisionEnter(o);
            });
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

        /// <summary>
        ///     Taken from https://www.youtube.com/watch?v=yYNrmsmEcy8 |
        ///     Rotates this object towards a given position.
        /// </summary>
        /// <param name="pos">From position</param>
        /// <param name="focus">The focus position, i.e what this game object will look at</param>
        /// <returns>A float representing the cartesian angle.</returns>
        public static float RotateTowards(Vector2 pos, Vector2 focus)
        {
            float h, sineTheta;
            var (x, y) = pos;
            if (y - focus.Y != 0)
            {
                h = (float) Math.Sqrt(Math.Pow(x - focus.X, 2) + Math.Pow(y - focus.Y, 2));
                sineTheta = Math.Abs(y - focus.Y) / h; //* ((item.Pos.Y-focus.Y)/(Math.Abs(item.Pos.Y-focus.Y))));
            }
            else
            {
                h = x - focus.X;
                sineTheta = 0;
            }

            var angle = (float) Math.Asin(sineTheta);

            // Drawing diagonial lines here.
            //Quadrant 2
            if (x - focus.X > 0 && y - focus.Y > 0)
                angle = (float) (Math.PI * 3 / 2 + angle);
            //Quadrant 3
            else if (x - focus.X > 0 && y - focus.Y < 0)
                angle = (float) (Math.PI * 3 / 2 - angle);
            //Quadrant 1
            else if (x - focus.X < 0 && y - focus.Y > 0)
                angle = (float) (Math.PI / 2 - angle);
            else if (x - focus.X < 0 && y - focus.Y < 0)
                angle = (float) (Math.PI / 2 + angle);
            else if (x - focus.X > 0 && y - focus.Y == 0)
                angle = (float) Math.PI * 3 / 2;
            else if (x - focus.X < 0 && y - focus.Y == 0)
                angle = (float) Math.PI / 2;
            else if (x - focus.X == 0 && y - focus.Y > 0)
                angle = 0;
            else if (x - focus.X == 0 && y - focus.Y < 0) angle = (float) Math.PI;

            return angle;
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
        /// <param name="other">The object that has collided with us.</param>
        protected virtual void OnCollisionEnter(GameObject other)
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