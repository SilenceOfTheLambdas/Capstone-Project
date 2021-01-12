#region Imports

using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace GrimGame.Engine
{
    public abstract class GameObject
    {
        protected GameObject(int x = 0, int y = 0, int w = 0, int h = 0, int id = 0)
        {
            // _____ Dimensions _____ //
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Bounds = new Rectangle(x, y, Width, Height);

            // _____ Properties _____ //
            Id = id;
            Active = true;
            Visible = true;
            Collision = false;
        }

        #region Properties

        public bool Active, Visible;

        /// <summary>
        ///     The position of this game object.
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(X, Y);
            protected set
            {
                X = value.X;
                Y = value.Y;
            }
        }

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
        protected Vector2 Velocity;

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
        ///     The name of this game object.
        /// </summary>
        public string Name;

        /// <summary>
        ///     The speed this object moves.
        /// </summary>
        public float Speed;

        /// <summary>
        ///     Assign a tag to this game object.
        /// </summary>
        public Globals.ObjectTags Tag;

        /// <summary>
        ///     The width of this game object.
        /// </summary>
        public int Width;

        // _____ Dimensions _____ //

        /// <summary>
        ///     The X and Y position of this game object.
        /// </summary>
        public float X, Y;

        /// <summary>
        ///     The ID of the object.
        /// </summary>
        private int Id { get; }

        // _____ Sprite _____ //

        /// <summary>
        ///     The sprite texture of this object
        /// </summary>
        protected Sprite Sprite;

        protected Texture2D Texture;

        protected Vector2 Scale;

        public BoxCollider BoxCollider;

        #endregion

        #region Methods

        /// <summary>
        ///     Here is where initialisation of the object is done.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///     Anything is this function will be ran every frame.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            CollisionDetection();
        }

        // _____ Setters _____ //
        /// <summary>
        ///     Set the position of this game object.
        /// </summary>
        /// <param name="x">The position on the x-axis</param>
        /// <param name="y">The position on the y-axis</param>
        public void SetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Set the size of the object.
        /// </summary>
        /// <param name="w">The width</param>
        /// <param name="h">The height</param>
        public void SetSize(int w, int h)
        {
            Width = w;
            Height = h;
        }

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
        public float DistanceTo(Vector2 from, Vector2 to)
        {
            return Vector2.Distance(from, to);
        }

        public Vector2 GetPositionCentered()
        {
            return new Vector2(X + Width / 2, X + Height / 2);
        }

        public void Destroy(GameObject gameObject)
        {
            SceneManager.GetActiveScene.ObjectManager.Remove(gameObject);
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
        public virtual void CollisionDetection()
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