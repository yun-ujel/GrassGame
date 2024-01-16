using UnityEditor;
using UnityEngine;

namespace GrassGame.Utilities
{
    public static class ScriptableObjectUtils
    {
        public static void Reimport(this ScriptableObject asset)
        {
            string path = AssetDatabase.GetAssetPath(asset.GetInstanceID());
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ImportRecursive);
        }
    }
}