using GrimGame.Game;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine
{
    public abstract class GameObject
    {
        // _____ Dimensions _____ //
        public float X, Y;
        public float XSpeed, YSpeed;
        public int Width, Height;
        public Vector2 Position { get => new Vector2(X, Y);
            set { X = value.X; Y = value.Y; }}
        public Vector2 Speed { get => new Vector2(XSpeed, YSpeed);
            set { XSpeed = value.X; YSpeed = value.Y; }}
        public Vector2 Size { get => new Vector2(Width, Height);
            set { Width = (int)value.X; Height = (int)value.Y; }}
        public Rectangle Bounds;
        
        // _____ Properties _____ //
        public int Id { get; }
        public string Text, Tag;
        public bool Active, Visible;
        public bool Collision, Hover;

        public GameObject(int x = 0, int y = 0, int w = 0, int h = 0, int id = 0)
        {
            // _____ Dimensions _____ //
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
            Bounds = new Rectangle(x, y, Width, Height);
            
            // _____ Properties _____ //
            this.Id = id;
            Active = true;
            Visible = true;
            Collision = false;
            Hover = false;
        }

        // _____ Abstract Methods _____ //
        /// <summary>
        /// Here is where initialisation of the object is done.
        /// </summary>
        /// <param name="g">A reference to the MainGame instance</param>
        public abstract void Init(MainGame g);
        /// <summary>
        /// Destroy *this* game object.
        /// </summary>
        /// <param name="g">A reference to the MainGame instance</param>
        public abstract void Destroy(MainGame g);
        /// <summary>
        /// Anything is this function will be ran every frame.
        /// </summary>
        /// <param name="g">A reference to the MainGame instance</param>
        public abstract void Update(MainGame g);
        /// <summary>
        /// Draws this object onto the screen.
        /// </summary>
        /// <param name="g">A reference to the MainGame instance</param>
        public abstract void Draw(MainGame g);

        // _____ Setters _____ //
        /// <summary>
        /// Set the position of this game object.
        /// </summary>
        /// <param name="x">The position on the x-axis</param>
        /// <param name="y">The position on the y-axis</param>
        public void SetPosition(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Sets the speed at which this object can move.
        /// </summary>
        /// <param name="xs">The speed going in the x direction</param>
        /// <param name="ys">The speed going in the y direction</param>
        public void SetSpeed(float xs, float ys)
        {
            this.XSpeed = xs;
            this.YSpeed = ys;
        }

        /// <summary>
        /// Set the size of the object.
        /// </summary>
        /// <param name="w">The width</param>
        /// <param name="h">The height</param>
        public void SetSize(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        /// <summary>
        /// Set the bounds of the object
        /// </summary>
        /// <param name="x">The x position of the bounding box</param>
        /// <param name="y">The y position of the bounding box</param>
        /// <param name="w">The width of the bounding box</param>
        /// <param name="h">The height of the bounding box</param>
        public void SetBounds(float x, float y, int w, int h)
        {
            this.Bounds = new Rectangle((int) x, (int) y, w, h);
        }
        
        // _____ Getters _____ //
        public float DistanceTo(Vector2 pos) => Vector2.Distance(Position, pos);
        public Vector2 GetPositionCentered() => new Vector2(X + (Width / 2), X + (Height / 2));
    }
}