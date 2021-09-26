using System;
using UnityEngine;

namespace Utility {
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour {
        public event Action<Collider> Enter;
        public event Action<Collider> Stay;
        public event Action<Collider> Exit;

        void OnTriggerEnter(Collider other) => Enter?.Invoke(other);

        void OnTriggerStay(Collider other) => Stay?.Invoke(other);

        void OnTriggerExit(Collider other) => Exit?.Invoke(other);
    }
}