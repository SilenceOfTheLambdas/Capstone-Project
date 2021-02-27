using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    public abstract class BtNode
    {
        public enum Result
        {
            Running,
            Failure,
            Success
        }

        /// <summary>
        ///     Executes a given task
        /// </summary>
        /// <returns>The result of the task; failure, running, success.</returns>
        public virtual Result Execute(GameTime gameTime)
        {
            return Result.Failure;
        }
    }
}