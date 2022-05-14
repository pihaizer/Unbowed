using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed.Utility {
    [ExecuteAlways]
    public class TransformFollow : MonoBehaviour {
        [SerializeField, Required] private Transform target;
        [SerializeField] private Vector3 offset;


        private void Update() {
            transform.position = offset + target.position;
            transform.rotation = target.rotation;
            transform.localScale = target.localScale;
        }
    }
}
