using System.Collections.Generic;
using GrimGame.Engine;
using GrimGame.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Game.Character
{
    public class Paladin : Enemy
    {
        public Paladin(Vector2 position)
        {
            Position = position;
        }

        public override void Init()
        {
            Sprite = new Sprite(new Dictionary<string, Animation>
            {
                {
                    "walk_up",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_up"), 2)
                },
                {
                    "walk_down",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_down"), 2)
                },
                {
                    "walk_left",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_left"), 2)
                },
                {
                    "walk_right",
                    new Animation(Globals.ContentManager.Load<Texture2D>("Sprites/Player/Animations/walk_right"), 2)
                }
            })
            {
                Width = 19,
                Height = 29
            };
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(),
                samplerState: SamplerState.PointClamp);

            Globals.SpriteBatch.End();
        }
    }
}