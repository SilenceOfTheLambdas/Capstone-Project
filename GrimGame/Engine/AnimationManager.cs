using GrimGame.Engine.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine
{
    /// <summary>
    ///     Handles all of the animations for a <seealso cref="GameObject" />.
    /// </summary>
    public class AnimationManager
    {
        private Animation _animation;
        private float     _timer;

        /// <summary>
        ///     Creates a new series of animations.
        /// </summary>
        /// <param name="animation">The animation to init</param>
        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        /// <summary>
        ///     The position of the frame.
        /// </summary>
        public Vector2 Position { get; set; }

        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }

        public float Rotation { get; set; }

        /// <summary>
        ///     Draws the current frame of a playing animation.
        /// </summary>
        public void Draw()
        {
            Globals.SpriteBatch.Draw(_animation.Texture,
                Position,
                new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                    0,
                    _animation.FrameWidth,
                    _animation.FrameHeight),
                Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.1f);
        }

        /// <summary>
        ///     Plays an animation.
        /// </summary>
        /// <param name="animation">The animation to play</param>
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        /// <summary>
        ///     Stop playing the currently playing animation.
        /// </summary>
        public void Stop()
        {
            _timer = 0;
            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0;

                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
    }
}