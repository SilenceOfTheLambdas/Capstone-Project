#region Imports

using System;
using GrimGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extended.Tiled;

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
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

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

        protected float Rotation { get; set; }

        public Vector2 Direction { get; set; }

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

        public static float GetDistance(Vector2 pos, Vector2 target)
        {
            return (float) Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
        }

        public static Vector2 RadialMovement(Vector2 focus, Vector2 pos, float speed)
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

        // public void MoveObjectAlongPath(Enemy p, ref Stack<Node> path)
        // {
        //     // arbitrary - todo: fix
        //     if (path.Count > 3)
        //     {
        //         Node next;
        //         next = path.Pop();
        //
        //         float X = next.Position.X - p.Position.X / Node.NODE_SIZE;
        //         float Y = (next.Position.Y - p.Position.Y / Node.NODE_SIZE);
        //         Vector2 vel = new Vector2(X, Y);
        //
        //         if (!(p.Position.X <= (next.Position.X * Node.NODE_SIZE * 1.0001f) && p.Position.Y <= (next.Position.Y * Node.NODE_SIZE * 1.0001f)
        //             && p.Position.X >= (next.Position.X * Node.NODE_SIZE * 0.9999f) && p.Position.Y >= (next.Position.Y * Node.NODE_SIZE * 0.9999f)))
        //         {
        //             MoveObject(p, vel);
        //         }
        //
        //         p.Position = next.Position * Node.NODE_SIZE;
        //         p.BoxCollider.Position = next.Position * Node.NODE_SIZE;
        //
        //         // if (vel.Y < 0)
        //         // {
        //         //     p.DirectionFacing = Direction.Up;
        //         // }
        //         // if (vel.X < 0)
        //         // {
        //         //     p.DirectionFacing = Direction.Left;
        //         // }
        //         // if (vel.Y > 0)
        //         // {
        //         //     p.DirectionFacing = Direction.Down;
        //         // }
        //         // if (vel.X > 0)
        //         // {
        //         //     p.DirectionFacing = Direction.Right;
        //         // }
        //
        //         //Console.Write("Moving enemy to: X = {0}, Y = {1}\n", p.Position.X, p.Position.Y);
        //         //p.Position = next.Position * 32;
        //         //p.EnemyRect.Position = next.Position * 32;
        //     }
        // }
        // public void MoveObject(Enemy p, Vector2 velocity)
        // {
        //     p.Position = p.Position;
        //     p.Velocity = velocity;
        //     double minT = TIMESTEP;
        //     float t = TIMESTEP;
        //     Vector2 mvA = new Vector2(0, 0);
        //     Vector2 mvB = new Vector2(0, 0);
        //     double ttc = 0.0;
        //     Vector2 minMV = new Vector2(0, 0);
        //     TCRectangle colRect = new TCRectangle();
        //
        //     while (t > MINESCULE_TIME)
        //     {
        //
        //         minMV = new Vector2(0, 0);
        //         minT = t;
        //         foreach (var rect in RectangleList)
        //         {
        //             if (BoxToBoxCollide(p.EnemyRect, rect, t, ref mvA, ref mvB, ref ttc))
        //             {
        //                 if (ttc < minT)
        //                 {
        //                     colRect = rect;
        //                     minT = ttc;
        //                     minMV = mvA;
        //                 }
        //             }
        //         }
        //
        //         minT -= MINESCULE_TIME;
        //         if (minT < 0) minT = 0;
        //
        //         p.EnemyRect.Position += p.EnemyRect.Velocity * (float)minT;
        //         p.EnemyRect.Velocity += minMV;
        //
        //         t -= (float)minT;
        //     }
        //
        //     p.Position = p.Bounds.ToRectangleF().Position;
        // }

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