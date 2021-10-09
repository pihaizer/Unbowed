using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Unbowed.Managers.Saves {
    public static class SaveController {
        public static void Save(SaveFile saveFile) {
            string destination = Application.persistentDataPath + "/save.json";
            File.WriteAllText(destination, JsonUtility.ToJson(saveFile, true));
        }

        public static SaveFile Load() {
            string destination = Application.persistentDataPath + "/save.json";
            if (File.Exists(destination)) {
                string json = File.ReadAllText(destination);
                var file = JsonUtility.FromJson<SaveFile>(json);
                return file;
            } else {
                return new SaveFile();
            }
        }
    }
}