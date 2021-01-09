using Microsoft.Xna.Framework.Graphics;

namespace GrimGame.Engine.Models
{
    public class Animation
    {
        public Animation(Texture2D texture, int frameCount, bool isLooping = true)
        {
            Texture = texture;
            FrameCount = frameCount;
            IsLooping = isLooping;
            FrameSpeed = 0.2f;
        }

        #region Variables

        public int CurrentFrame { get; set; }
        public int FrameCount { get; }
        public int FrameHeight => Texture.Height;
        public float FrameSpeed { get; }
        public int FrameWidth => Texture.Width / FrameCount;
        public bool IsLooping { get; set; }
        public Texture2D Texture { get; }

        #endregion
    }
}