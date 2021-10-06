using UnityEngine;
namespace Unbowed.SO {
    [CreateAssetMenu(fileName = "Scene Config", menuName = "Configs/Scene Config", order = 0)]
    public class SceneConfig : ScriptableObject {
        public string sceneName;
    }
}