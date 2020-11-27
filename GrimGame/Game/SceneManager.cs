#region Imports

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

#endregion

namespace GrimGame.Game
{
    public static class SceneManager
    {
        private static readonly List<Scene> Scenes = new List<Scene>();

        public static void AddScene(Scene scene) => Scenes.Add(scene);

        public static void LoadScene(string name)
        {
            foreach (var scene in Scenes.Where(scene => scene.Name.ToLower().Equals(name.ToLower())))
            {
                scene.SetIsSceneLoaded(true);
            }
        }

        public static void InitScenes()
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded()))
            {
                scene.Initialize();
            }
        }

        public static void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded()))
            {
                scene.Update(gameTime);
            }
        }

        public static void DrawScenes(GameTime gameTime)
        {
            foreach (var scene in Scenes.Where(scene => scene.GetIsSceneLoaded()))
            {
                scene.Draw(gameTime);
            }
        }
    }
}