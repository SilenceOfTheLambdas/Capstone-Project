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

        public void Update(MainGame g)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = Objects[i];

                if (obj.Active)
                {
                    obj.SetBounds(obj.X, obj.Y, obj.Width, obj.Height);
                    obj.Update(g);
                }
            }
        }

        public void Draw(MainGame g)
        {
            for (int i = 0; i < Count; i++)
            {
                var obj = Objects[i];

                if (obj.Active && obj.Visible)
                {
                    obj.Draw(g);
                }
            }
        }

        /// <summary>
        /// Add a new game object to the list.
        /// </summary>
        /// <param name="obj">The GameObject to add</param>
        /// <param name="g">A reference to the MainGame</param>
        public void Add(GameObject obj, MainGame g)
        {
            Objects.Add(obj);
            obj.Init(g);
        }

        /// <summary>
        /// Remove a GameObject from the scene and list.
        /// </summary>
        /// <param name="obj">The GameObject to remove</param>
        /// <param name="g">A reference to the MainGame</param>
        public void Remove(GameObject obj, MainGame g)
        {
            obj.Destroy(g);
            Objects.Remove(obj);
        }

        /// <summary>
        /// Removes a GameObject from the scene and list.
        /// </summary>
        /// <param name="index">The list index of the GameObject to remove</param>
        /// <param name="g">A reference to the MainGame</param>
        public void Remove(int index, MainGame g)
        {
            Objects[index].Destroy(g);
            Objects.Remove(Objects[index]);
        }
        
        /// <summary>
        /// Clear the list of all GameObjects.
        /// </summary>
        public void Clear() { Objects.Clear();}
    }
}