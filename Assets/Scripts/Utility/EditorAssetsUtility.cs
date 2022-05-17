#if UNITY_EDITOR
using System.Collections.Generic;
using Unbowed.Gameplay.Characters.Stats.Configs;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Utility
{
    public static class EditorAssetsUtility
    {
        public static List<T> FindAssetsOfType<T>() where T : Object
        {
            List<T> assets = new();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                assets.Add(asset);
            }

            return assets;
        }
    }
}
#endif