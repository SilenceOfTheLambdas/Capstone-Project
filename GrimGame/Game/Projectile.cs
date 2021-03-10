using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GrimGame.Game
{
    public class Projectile : GameObject
    {
        public int Damage { get; }

        public Projectile(float radius, int damage)
        {
            Damage = damage;
            var circle = new CircleF(Position, radius);
            BoxCollider = new BoxCollider(circle.Center, new Point2(radius * 2, radius * 2));
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity;

            BoxCollider.UpdatePosition(Position.ToPoint());
        }

        public override void Draw()
        {
            // Begin a new sprite batch call
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: new SamplerState {Filter = TextureFilter.Point});
            // Draw a circle using the MonoGame.Extended DrawCircle method
            Globals.SpriteBatch.DrawCircle(new CircleF(Position, 10f), 60, Color.Red);
            // End the spritebatch
            Globals.SpriteBatch.End();
        }
    }
}