#region Imports

using GrimGame.Game;
using Microsoft.Xna.Framework;

#endregion

namespace GrimGame.Engine
{
    public abstract class GameObject
    {
        public bool Active, Visible;

        /// <summary>
        ///     The bounding box for this game object.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        ///     Is this object colliding with something?
        /// </summary>
        public bool Collision = false;

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
        ///     The origin point of this game object.
        /// </summary>
        protected Vector2 Origin;

        /// <summary>
        ///     The speed this object moves.
        /// </summary>
        public float Speed;

        /// <summary>
        ///     Assign a tag to this game object.
        /// </summary>
        public Globals.ObjectTags Tag;

        /// <summary>
        ///     The velocity at which this objects moves.
        /// </summary>
        protected Vector2 Velocity;

        /// <summary>
        ///     The width of this game object.
        /// </summary>
        public int Width;

        // _____ Dimensions _____ //
        /// <summary>
        ///     The X and Y position of this game object.
        /// </summary>
        public float X, Y;

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
        ///     The ID of the object.
        /// </summary>
        public int Id { get; }

        // _____ Sprite _____ //
        /// <summary>
        ///     The sprite texture of this object
        /// </summary>
        public Sprite Sprite { get; protected set; }

        // _____ Abstract Methods _____ //
        /// <summary>
        ///     Here is where initialisation of the object is done.
        /// </summary>
        public abstract void Init();

        /// <summary>
        ///     Anything is this function will be ran every frame.
        /// </summary>
        /// <param name="scene">A reference to a game scene instance</param>
        /// <param name="gameTime"></param>
        public abstract void Update(Scene scene, GameTime gameTime);

        /// <summary>
        ///     Draws this object onto the screen.
        /// </summary>
        /// <param name="g">A reference to the MainGame instance</param>
        public abstract void Draw();

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
    }
}