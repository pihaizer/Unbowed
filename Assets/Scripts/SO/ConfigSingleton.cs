using Unbowed.Managers;

using UnityEngine;

namespace Unbowed.SO {
    public abstract class ConfigSingleton : ScriptableObject {}
    public abstract class ConfigSingleton<T> : ConfigSingleton where T : ConfigSingleton<T> {
        public static T Instance {
            get {
                return (T)ConfigsContainer.Instance.configs.Find(config => config is T);
            }
        }
    }
}