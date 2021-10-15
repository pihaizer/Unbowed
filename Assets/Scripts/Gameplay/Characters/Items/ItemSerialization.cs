using UnityEngine;

namespace Unbowed.Gameplay.Characters.Items {
    public partial class Item : ISerializationCallbackReceiver {
        [SerializeField, HideInInspector]
        string configName;
        
        public void OnBeforeSerialize() => configName = _config != null ? _config.name : null;

        public void OnAfterDeserialize() { }
    }
}