using System.Collections.Generic;
using GrimGame.Engine;
using Microsoft.Xna.Framework;

namespace GrimGame.Game
{
    /// <summary>
    ///     This class is responsible for handling all of the game objects in a scene.
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        ///     List of game objects in the scene.
        /// </summary>
        public List<GameObject> Objects { get; } = new List<GameObject>();

        /// <summary>
        ///     The number of game objects in the scene.
        /// </summary>
        private int Count => Objects.Count;

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < Count; i++)
            {
                var obj = Objects[i];

                if (obj.Active)
                {
                    obj.SetBounds(obj.X, obj.Y, obj.Width, obj.Height);
                    obj.Update(gameTime);

                    // if (Count >= 2)
                    // {
                    //     var nextObj = Objects[(i + 1) % Count];
                    //     if (obj is Player player && nextObj is Enemy enemy)
                    //     {
                    //         Console.WriteLine($"Player colliding? {player.Collision}");
                    //         if (player.IsColliding(ref enemy.BoxCollider.Bounds))
                    //         {
                    //             player.OnCollisionEnter(nextObj);
                    //             player.Collision = true;
                    //         
                    //             nextObj.OnCollisionEnter(obj);
                    //             nextObj.Collision = true;
                    //         }
                    //         // If the player was colliding, and now is not
                    //         if (player.Collision && !obj.IsColliding(ref enemy.BoxCollider.Bounds))
                    //         {
                    //             player.OnCollisionExit();
                    //             player.Collision = false;
                    //         }
                    //     }
                    // }
                }
            }
        }

        /// <summary>
        ///     Add a new game object to the list.
        /// </summary>
        /// <param name="obj">The GameObject to add</param>
        public void Add(GameObject obj)
        {
            Objects.Add(obj);
        }

        /// <summary>
        ///     Remove a GameObject from the scene and list.
        /// </summary>
        /// <param name="obj">The GameObject to remove</param>
        public void Remove(GameObject obj)
        {
            Objects.Remove(obj);
        }

        /// <summary>
        ///     Clear the list of all GameObjects.
        /// </summary>
        public void Clear()
        {
            Objects.Clear();
        }
    }
}