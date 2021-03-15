#region Imports

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

#endregion

namespace GrimGame.Game
{
    /// <summary>
    ///     Manages every <see cref="Scene" /> in the game.
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        ///     List of every scene
        /// </summary>
        private static readonly List<Scene> Scenes = new List<Scene>();

        /// <summary>
        ///     Get the currently loaded and active scene.
        /// </summary>
        public static Scene GetActiveScene
        {
            get { return Scenes.First(scene => scene.GetIsSceneLoaded()); }
        }

        /// <summary>
        ///     Add a new scene to the game.
        /// </summary>
        /// <param name="scene">Scene to add</param>
        public static void AddScene(Scene scene)
        {
            Scenes.Add(scene);
        }

        /// <summary>
        ///     Load a new scene.
        /// </summary>
        /// <param name="name">The <see cref="string" /> name of the scene to load</param>
        public static void LoadScene(string name)
        {
            foreach (var scene in Scenes)
            {
                if (scene.Name.ToLower().Equals(name.ToLower()))
                {
                    scene.SetIsSceneLoaded(true);
                    scene.Initialize();
                }
                else
                {
                    scene.ObjectManager.Clear();
                    scene.SetIsSceneLoaded(false);
                    scene.Initialize();
                }
            }
        }

        /// <summary>
        /// Call <see cref="Scene.Initialize()"/> for every scene
        /// </summary>
        public static void InitScenes()
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded())) scene.Initialize();
        }

        /// <summary>
        /// Calls <see cref="Scene.Update(GameTime)"/> for every scene
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public static void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded())) scene.Update(gameTime);
        }

        /// <summary>
        /// Draws every scene by calling it's <see cref="Scene.Draw"/> method
        /// </summary>
        public static void DrawScenes()
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded())) scene.Draw();
        }
    }
}