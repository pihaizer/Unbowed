using System;
using UnityEngine;

namespace SO.Events {
    [CreateAssetMenu(fileName = "SceneLoadRequestEventSO", menuName = "SO/Events/SceneLoadRequestEvent")]
    public class SceneLoadRequestEventSO : ParameterEventSO<SceneLoadRequestEventSO.SceneLoadRequestData> {
        [Serializable]
        public struct SceneLoadRequestData {
            public string name;
            public bool isLoad;
            public bool hasLoadScreen;
        }
    }
}