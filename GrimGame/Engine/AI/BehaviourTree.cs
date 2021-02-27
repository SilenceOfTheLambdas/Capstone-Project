using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    public class BehaviourTree
    {
        private bool _startedBehaviour;

        /// <summary>
        ///     Memory space
        /// </summary>
        public Dictionary<string, object> Blackboard { get; private set; }

        private BtNode Root { get; set; }

        public void Init()
        {
            Blackboard = new Dictionary<string, object> {{"WorldBounds", new Rectangle(0, 0, 5, 5)}};
            _startedBehaviour = false;

            // set the root behaviour
            // TODO 
        }
    }
}