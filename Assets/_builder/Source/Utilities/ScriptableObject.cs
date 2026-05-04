using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace nobodyworks.builder.utilities
{
    public static partial class Utils
    {
        #region Editor

#if UNITY_EDITOR

        public static T[] GetAssets<T>()
            where T : ScriptableObject
        {
            var typeName = typeof(T).Name;
            var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");
            var assets = new T[assetsGuids.Length];

            for (int i = 0; i < assetsGuids.Length; ++i)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetsGuids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            }

            return assets;
        }

        public static T GetAsset<T>()
            where T : ScriptableObject
        {
            var typeName = typeof(T).Name;
            var assetsGuids = AssetDatabase.FindAssets($"t:{typeName}");

            if (assetsGuids.Length <= 0)
            {
                return null;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(assetsGuids[0]);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            return asset;
        }
        
#endif

        #endregion
    }
}
