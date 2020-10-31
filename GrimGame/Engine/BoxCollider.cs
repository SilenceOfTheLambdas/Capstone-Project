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
        
        private readonly GameObject GameObject;

        public BoxCollider(GameObject gameObject, Vector2 origin, Point2 size)
        {
            this.GameObject = gameObject;
            this.Origin = origin;
            Size = size;
        }

        public override void Init(MainGame g)
        {
            Bounds = new Rectangle(new Point((int) (Origin.X), (int) Origin.Y), new Point((int)Size.X, (int)Size.Y));
        }

        public override void Update(MainGame g)
        {
            Bounds = new Rectangle(new Point((int) (Origin.X), (int) Origin.Y), new Point((int)Size.X, (int)Size.Y));
        }

        public void UpdatePosition(Point position)
        {
            Bounds.Location = position;
        }
        
        public override void Draw(MainGame g)
        {
            throw new System.NotImplementedException();
        }
    }
}