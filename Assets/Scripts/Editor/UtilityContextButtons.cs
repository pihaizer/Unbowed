using UnityEditor;

using UnityEngine;

namespace Unbowed.Editor {
    public static class UtilityContextButtons {
        [MenuItem("Utility/OpenSavePath")]
        public static void OpenSavePath() {
            // explorer doesn't like front slashes
            string itemPath = Application.persistentDataPath.Replace(@"/", @"\");   
            System.Diagnostics.Process.Start("explorer.exe", itemPath);
        }
    }
}