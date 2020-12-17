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
        private readonly List<GameObject> _objects = new List<GameObject>();

        /// <summary>
        /// The number of game objects in the scene.
        /// </summary>
        private int Count => _objects.Count;

        public void Update(Scene scene)
        {
            for (var i = 0; i < Count; i++)
            {
                var obj = _objects[i];

                if (obj.Active)
                {
                    obj.SetBounds(obj.X, obj.Y, obj.Width, obj.Height);
                    obj.Update(scene);
                }
            }
        }

        /// <summary>
        /// Add a new game object to the list.
        /// </summary>
        /// <param name="obj">The GameObject to add</param>
        public void Add(GameObject obj)
        {
            _objects.Add(obj);
        }

        /// <summary>
        /// Remove a GameObject from the scene and list.
        /// </summary>
        /// <param name="obj">The GameObject to remove</param>
        public void Remove(GameObject obj)
        {
            _objects.Remove(obj);
        }

        /// <summary>
        /// Clear the list of all GameObjects.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
        }
    }
}