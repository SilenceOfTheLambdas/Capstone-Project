using System.Collections.Generic;
using GrimGame.Engine.Models;

namespace GrimGame.Engine
{
    /// <summary>
    ///     A sprite is a Texture2D image that controls it's animations.
    /// </summary>
    public sealed class Sprite
    {
        /// <summary>
        ///     A list of animations that this sprite can perform.
        /// </summary>
        public readonly Dictionary<string, Animation> Animations;

        /// <summary>
        ///     Creates a new sprite.
        /// </summary>
        /// <param name="animations">A <see cref="Dictionary{TKey,TValue}" /> of animations, {name, Animation}</param>
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