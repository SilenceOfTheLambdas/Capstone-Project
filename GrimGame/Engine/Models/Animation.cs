using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.Models
{
    /// <summary>
    ///     An animation is a <see cref="Texture2D" /> that holds different frames. These frames are split, and the switched
    ///     every n frames. This provides an animation.
    ///     <seealso cref="AnimationManager" />
    /// </summary>
    public class Animation
    {
        /// <summary>
        ///     Create a new animation strip.
        /// </summary>
        /// <param name="texture">The animation strip</param>
        /// <param name="frameCount">The number of frames this animation has</param>
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            FrameSpeed = 0.2f;
        }

        #region Variables

        /// <summary>
        ///     The current frame of this animation.
        /// </summary>
        public int CurrentFrame { get; set; }

        /// <summary>
        ///     Total frame count.
        /// </summary>
        public int FrameCount { get; }

        /// <summary>
        ///     The height of the frame.
        /// </summary>
        public int FrameHeight => Texture.Height;

        /// <summary>
        ///     The speed that each frame is transitioned to.
        /// </summary>
        public float FrameSpeed { get; }

        /// <summary>
        ///     Width of the frame.
        /// </summary>
        public int FrameWidth => Texture.Width / FrameCount;

        /// <summary>
        ///     Get the <see cref="Texture2D" /> of this animation
        /// </summary>
        public Texture2D Texture { get; }

        #endregion
    }
}