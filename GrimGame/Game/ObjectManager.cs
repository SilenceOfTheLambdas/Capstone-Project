using System.Collections.Generic;
using GrimGame.Engine;

namespace GrimGame.Game
{
    /// <summary>
    /// This class is responsible for handling all of the game objects in a scene.
    /// </summary>
    public class ObjectManager
    {
        /// <summary>
        /// List of game objects in the scene.
        /// </summary>
        public List<GameObject> Objects = new List<GameObject>();
        
        /// <summary>
        /// The number of game objects in the scene.
        /// </summary>
        public int Count => Objects.Count;
        
        public ObjectManager() {}

        public void Update(Scene scene)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = Objects[i];

                if (obj.Active)
                {
                    obj.SetBounds(obj.X, obj.Y, obj.Width, obj.Height);
                    obj.Update(scene);
                }
            }
        }

        public void Draw(MainGame g)
        {
            
        }

        /// <summary>
        /// Add a new game object to the list.
        /// </summary>
        /// <param name="obj">The GameObject to add</param>
        /// <param name="g">A reference to the MainGame</param>
        public void Add(GameObject obj, MainGame g)
        {
            Objects.Add(obj);
        }

        /// <summary>
        /// Remove a GameObject from the scene and list.
        /// </summary>
        /// <param name="obj">The GameObject to remove</param>
        /// <param name="g">A reference to the MainGame</param>
        public void Remove(GameObject obj)
        {
            Objects.Remove(obj);
        }
        
        /// <summary>
        /// Clear the list of all GameObjects.
        /// </summary>
        public void Clear() { Objects.Clear();}
    }
}