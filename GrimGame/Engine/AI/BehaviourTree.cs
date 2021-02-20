using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace GrimGame.Engine.AI
{
    public class BehaviourTree
    {
        private bool _startedBehaviour;
        
        /// <summary>
        /// Memory space
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