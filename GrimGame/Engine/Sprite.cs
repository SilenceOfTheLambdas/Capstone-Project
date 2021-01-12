using System.Collections.Generic;
using GrimGame.Engine.Models;

namespace GrimGame.Engine
{
    public sealed class Sprite
    {
        public readonly Dictionary<string, Animation> Animations;

        public Sprite(Dictionary<string, Animation> animations)
        {
            Animations = animations;
        }

        #region Properties

        public int Width { get; set; }
        public int Height { get; set; }

        #endregion
    }
}