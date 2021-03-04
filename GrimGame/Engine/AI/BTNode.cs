using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    /// <summary>
    ///     Represents a behaviour node, all node types extend this.
    /// </summary>
    public abstract class BtNode
    {
        /// <summary>
        ///     A list of return types/values.
        /// </summary>
        public enum Result
        {
            /// <summary>
            ///     This process is still running it's behaviour.
            /// </summary>
            Running,

            /// <summary>
            ///     This behaviour has failed.
            /// </summary>
            Failure,

            /// <summary>
            ///     This behaviour has succeeded.
            /// </summary>
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