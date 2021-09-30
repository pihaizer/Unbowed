using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Utility {
    [ExecuteAlways]
    public class TransformFollow : MonoBehaviour {
        [SerializeField, Required] Transform target;
        [SerializeField] Vector3 offset;
    
        
        void Update() {
            transform.position = offset + target.position;
            transform.rotation = target.rotation;
            transform.localScale = target.localScale;
        }
    }
}
