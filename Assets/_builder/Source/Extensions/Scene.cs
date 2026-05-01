using UnityEngine.SceneManagement;

namespace nobodyworks.builder.extensions
{
    public static class SceneExtensions
    {
        public static void SetActive(this Scene scene, bool active)
        {
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                gameObject.SetActive(active);
            }
        }
    }
}