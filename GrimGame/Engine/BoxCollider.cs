#region Imports
using GrimGame.Game;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
#endregion

namespace GrimGame.Engine
{
    public class BoxCollider : GameObject
    {
        /// <summary>
        /// Does this collider trigger an event?
        /// </summary>
        public bool IsTrigger = false;
        

        public BoxCollider(Vector2 origin, Point2 size)
        {
            this.Origin = origin;
            Size = size;
        }

        public override void Init()
        {
            Bounds = new Rectangle(new Point((int) (Origin.X), (int) Origin.Y), new Point((int)Size.X, (int)Size.Y));
        }

        public override void Update(Game.Scene scene)
        {
            Bounds = new Rectangle(new Point((int) (Origin.X), (int) Origin.Y), new Point((int)Size.X, (int)Size.Y));
        }

        public void UpdatePosition(Point position)
        {
            Bounds.Location = position;
        }
        
        public override void Draw()
        {
            
        }
    }
}